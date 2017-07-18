#include <stdio.h>
#include <stdlib.h>

// RIFFチャンクおよびWAVEフォームタイプ構造体
typedef struct{
	char id[4];				// "RIFF"
	unsigned long size;		// ファイルサイズ - 8
	char form[4];			// "WAVE"
} riff_chunk;

// fmtチャンク構造体
typedef struct{
	char id[4];					// "fmt "
	unsigned long size;			// fmt領域のサイズ
	unsigned short format_id;	// フォーマットID (PCM:1)
	unsigned short channel;		// チャネル数 (モノラル:1 ステレオ:2)
	unsigned long  fs;			// サンプリング周波数
	unsigned long  byte_sec;	// １秒あたりのバイト数（fs * byte_samp）
	unsigned short byte_samp;	// 1要素あたりのバイト数（channel(bit/8)）
	unsigned short bit;			// 量子化ビット数（8 or 16）
} fmt_chunk;

// dataチャンク構造体
typedef struct{
	char id[4];				// "data"
	unsigned long size;		// data領域のサイズ
} data_chunk;

// グローバル変数を宣言
riff_chunk riff = {0};		//riff_chunk型で変数riffを宣言
fmt_chunk fmt = {0};		//fmt_chuck型で変数fmtを宣言
data_chunk data = {0};		//data_chunk型で変数dataを宣言
int mode;

void display(riff_chunk, fmt_chunk, data_chunk);
int countRow(char[], FILE*, unsigned short);
short* readTextfile(FILE*, unsigned short, short[]);
void makeHead(unsigned short);
void readHead(FILE* fr);
void writeData(FILE*, unsigned short, short*);
FILE* wfileopen(void);
FILE* rfileopen(void);
FILE* wbfileopen(void);
FILE* rbfileopen(void);
void modeSelect(void);


// main
int main()
{
	int i;
	unsigned short count = 0;
	FILE* fp;
	FILE* fr;
	short *heap = 0;
	short pcm_data;
	char buff[256];

	// やることを選択する。
	modeSelect();

	// text-waveの場合
	if(mode == 1){
		printf("text->waveを行います。\n");
		printf("textファイルを読み込みます。\n");
		fr = rfileopen();
		count = countRow(buff, fr, count);
		heap = (short *)malloc(sizeof(unsigned short) * count);
		if (heap == NULL) exit(-1);
		readTextfile(fr, count, heap);
		fp = wbfileopen();
		makeHead(count);
		writeData(fp, count, heap);
		printf("書き込み完了。\n");
		fclose(fp);
		fclose(fr);
		free(heap);
	}

	// wave->textの場合
	if(mode == 2){
		printf("wave->textを行います。\n");
		printf("WAVEファイルを読み込みます。\n");
		fr = rbfileopen();
		readHead(fr);
		printf("textファイルに書き込みます。\n");
		fp = wbfileopen();
		for(i = 0; i < data.size / 2; i++){
			fread(&pcm_data, sizeof(short), 1, fr);
			fprintf(fp, "%d\n", pcm_data);
		}
		fclose(fp);
		fclose(fr);
		printf("書き込み完了。\n");
	}
}


// 表示する。
void display(riff_chunk riff, fmt_chunk fmt, data_chunk data){
	printf("filesize:%ld[byte]\n", riff.size);		// ファイルサイズを表示
	printf("channel:%d\n", fmt.channel);				// チャネル数を表示
	printf("fs:%lu[Hz]\n", fmt.fs);					// サンプリング周波数を表示
	printf("bit:%d[bit]\n", fmt.bit);				// 量子化ビットを表示
	printf("data:%lu[sample]\n", data.size / 2);		// データ数を表示
	printf("time:%lf[s]\n", data.size / 2.0 / fmt.fs);	// 時間を表示
}

// 行数をカウントする。
int countRow(char buff[], FILE* fp, unsigned short count){
	while(fgets(buff,256,fp) != NULL) count++ ;
	fseek(fp,0,SEEK_SET);
	return count;
}

// txtデータを読み込む。
short* readTextfile(FILE* fp, unsigned short count, short pcm_data[]){
	int i;
	for(i = 0; i < count; i++) fscanf(fp, "%hd", &pcm_data[i]);
	return pcm_data;
}

// ヘッダを作成する。
void makeHead(unsigned short count){
	riff.id[0] = 'R';
	riff.id[1] = 'I';
	riff.id[2] = 'F';
	riff.id[3] = 'F';
	riff.size = count * 2 + 36;
	riff.form[0] = 'W';
	riff.form[1] = 'A';
	riff.form[2] = 'V';
	riff.form[3] = 'E';
	fmt.id[0] = 'f';
	fmt.id[1] = 'm';
	fmt.id[2] = 't';
	fmt.id[3] = ' ';
	fmt.size = 16;
	fmt.format_id = 1;
	fmt.channel = 1;
	fmt.fs = 11025;
	fmt.byte_sec = 22050;
	fmt.byte_samp = 2;
	fmt.bit = 16;
	data.id[0] = 'd';
	data.id[1] = 'a';
	data.id[2] = 't';
	data.id[3] = 'a';
	data.size = count*2;
}

// ヘッダを読み込む。
void readHead(FILE* fr){
	fseek(fr,0,SEEK_SET);
	fread(&riff.id[0], sizeof(char), 1, fr);
	fread(&riff.id[1], sizeof(char), 1, fr);
	fread(&riff.id[2], sizeof(char), 1, fr);
	fread(&riff.id[3], sizeof(char), 1, fr);
	fread(&riff.size, sizeof(unsigned long), 1, fr);
	fread(&riff.form[0], sizeof(char), 1, fr);
	fread(&riff.form[1], sizeof(char), 1, fr);
	fread(&riff.form[2], sizeof(char), 1, fr);
	fread(&riff.form[3], sizeof(char), 1, fr);
	fread(&fmt.id[0], sizeof(char), 1, fr);
	fread(&fmt.id[1], sizeof(char), 1, fr);
	fread(&fmt.id[2], sizeof(char), 1, fr);
	fread(&fmt.id[3], sizeof(char), 1, fr);
	fread(&fmt.size, sizeof(unsigned long), 1, fr);
	fread(&fmt.format_id, sizeof(unsigned short), 1, fr);
	fread(&fmt.channel, sizeof(unsigned short), 1, fr);
	fread(&fmt.fs, sizeof(unsigned long), 1, fr);
	fread(&fmt.byte_sec, sizeof(unsigned long), 1, fr);
	fread(&fmt.byte_samp, sizeof(unsigned short), 1, fr);
	fread(&fmt.bit, sizeof(unsigned short), 1, fr);
	fread(&data.id[0], sizeof(char), 1, fr);
	fread(&data.id[1], sizeof(char), 1, fr);
	fread(&data.id[2], sizeof(char), 1, fr);
	fread(&data.id[3], sizeof(char), 1, fr);
	fread(&data.size, sizeof(unsigned long), 1, fr);
}

// データを書き込む。
void writeData(FILE *fp, unsigned short count, short* heap){
	int i;
	fwrite(&riff, sizeof(riff_chunk), 1, fp);
	fwrite(&fmt, sizeof(fmt_chunk), 1, fp);
	fwrite(&data, sizeof(data_chunk), 1, fp);
	for(i = 0; i < count; i++) fwrite(&heap[i], sizeof(short), 1, fp);
}

// 書き込みモード
FILE* wfileopen(){
	FILE* fp;
	char filename[256];
	printf("書き込むファイル名\n--> ");
	scanf("%s", filename);
	fp = fopen(filename, "w");
	if(fp == NULL){
		printf("can't open file.\n");
		exit(-1);
	}
	return fp;
}

// 読み込みモード
FILE* rfileopen(){
	FILE* fr;
	char filename[256];
	printf("読み込むファイル名\n--> ");
	scanf("%s", filename);
	fr = fopen(filename, "r");
	if(fr == NULL){
		printf("can't open file.\n");
		exit(-1);
	}
	return fr;
}

// バイナリ書き込みモード
FILE* wbfileopen(){
	FILE* fp;
	char filename[256];
	printf("書き込むファイル名\n--> ");
	scanf("%s", filename);
	fp = fopen(filename, "wb");
	if(fp == NULL){
		printf("can't open file.\n");
		exit(-1);
	}
	return fp;
}

// バイナリ読み込みモード
FILE* rbfileopen(){
	FILE* fr;
	char filename[256];
	printf("読み込むファイル名\n--> ");
	scanf("%s", filename);
	fr = fopen(filename, "rb");
	if(fr == NULL){
		printf("can't open file.\n");
		exit(-1);
	}
	return fr;
}

// やることを選択
void modeSelect(){
	int flag = 0;
	while(flag == 0){
		printf("text->wave : 1\nwave->text : 2\n->  ");
		scanf("%d", &mode);
		if(mode == 1 || mode == 2){
			flag = 1;
		}else{
			printf("1か２を入力して下さい。\n");
		}
	}
}
