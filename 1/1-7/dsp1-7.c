#include <stdio.h>
#include <stdlib.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <time.h>

#define MAX_SIZE 16384

// 実部と虚部を扱う構造体
typedef struct {
	double re;	// 実部
	double im;	// 虚部
} complex_t;

complex_t addComp(complex_t, complex_t);
complex_t subComp(complex_t, complex_t);
complex_t multiComp(complex_t, complex_t);
complex_t divComp(complex_t, complex_t);
complex_t conjComp(complex_t);
void twid(complex_t*, int);
void bitRev(int[], int);
void convertRev(complex_t[], int[], int);
void butterfly(complex_t*, complex_t*, int);
FILE* wfileopen(void);
FILE* rfileopen(void);
void ampSpectrum(complex_t[], int);


int main(int argc, char const *argv[])
{
	int i;
	int N = 0;
	int count = 0;
	int bit[MAX_SIZE] = {0};
	char tmp[256];
	FILE* fp;
	complex_t Xn[MAX_SIZE], Xk[MAX_SIZE];

	// ファイルオープン
	fp = rfileopen();

	// データ数をカウント
	while(fgets(tmp, 252, fp) != NULL) count++;

	// 何点で行うかチェック
	while(N == 0) {
		printf("何点で行いますか。\n--> ");
		scanf("%d", &N);
		if(N > count || N < 0){
			printf("正しい範囲で値を入力してください。\n");
			N = 0;
		}
	}

	// 初期化
	for(i = 0; i < MAX_SIZE; i++){
		Xn[i].re = 0;
		Xn[i].im = 0;
		Xk[i].re = 0;
		Xk[i].im = 0;
	}

	// データ読み込み
	fseek(fp, 0L, SEEK_SET);
	for(i = 0; i < count; i++) fscanf(fp, "%lf", &Xn[i].re);
	fclose(fp);

	// FFT
	twid(Xk, N); 			// 回転子生成
	bitRev(bit, N); 		// ビットリバーサル
	convertRev(Xn, bit, N); 	// 配列データ入れ替え
	butterfly(Xn, Xk, N); 	// バタフライ演算

	// ファイル書き込み
	fp = wfileopen();
	printf("FFT結果をファイルに出力します。\n");
	for(i = 0; i < N; i++) fprintf(fp, "%.6lf %.6lf\n", Xn[i].re, Xn[i].im);
	fclose(fp);

	// スペクトル
	// ampSpectrum(Xn,N);

	return 0;
}



// 複素数の足し算
complex_t addComp(complex_t Xk, complex_t Yk){
	complex_t result;

	result.re = Xk.re + Yk.re;
	result.im = Xk.im + Yk.im;
	return result;
}

// 複素数の引き算
complex_t subComp(complex_t Xk, complex_t Yk){
	complex_t result;

	result.re = Xk.re - Yk.re;
	result.im = Xk.im - Yk.im;
	return result;
}

// 複素数の掛け算
complex_t multiComp(complex_t Xk, complex_t Yk){
	complex_t result;

	result.re = (Xk.re * Yk.re) - (Xk.im * Yk.im);
	result.im = (Xk.im * Yk.re) + (Xk.re * Yk.im);
	return result;
}

// 複素数の割り算
complex_t divComp(complex_t Xk, complex_t Yk){
	complex_t result;

	result.re = (Xk.re * Yk.re + Xk.im * Yk.im) / (pow(Yk.re,2.0) + pow(Yk.im,2.0));
	result.im = (Xk.im * Yk.re - Xk.re * Yk.im) / (pow(Yk.re,2.0) + pow(Yk.im,2.0));
	return result;
}

// 共役複素数
complex_t conjComp(complex_t Xk){
	complex_t result;

	result.re =  Xk.re;
	result.im = -1 * Xk.im;
	return result;
}

// 回転子
void twid(complex_t* wnk, int N){
	int i;
	double rad;
	rad = 2 * M_PI / N;

	for(i = 0; i < N; i++){
		wnk[i].re = cos(rad * i);
		wnk[i].im = sin(-1 * rad * i);
	}
}

// ビットリバース
void bitRev(int bit[], int N){
	int i, j, r;
	r = (int)(log(N) / log(2.0) + 0.5);

	for(i = 0; i < N; i++){
		bit[i] = 0;
		for(j = 0; j < r; j++){
			 bit[i] += ((i >> j ) & 1) << (r-j-1);
		}
	}
}

// 配列作成
void convertRev(complex_t in[], int bit[], int N){
	int i;
	complex_t tmp[N];

	for(i = 0; i < N; i++){
		tmp[i].re = in[i].re;
		tmp[i].im = in[i].im;
	}
	for (i = 0; i < N; i++) {
		in[i].re = tmp[bit[i]].re;
		in[i].im = tmp[bit[i]].im;
	}
}

// バタフライ演算
void butterfly(complex_t *in, complex_t *wnk, int N){
	int r_big = 1, r_sma = N / 2;
	int i,j,k,r,in1,in2,nk;
	complex_t tmp;
	clock_t start, end;
	r = (int)(log(N) / log(2.0) + 0.5);

	start = clock();
	for(i = 0; i < r; i++){
		for(j = 0; j < r_big; j++){
			for(k = 0; k < r_sma; k++){
				in1 = r_big * 2 * k + j;
				in2 = in1 + r_big;
				nk = j * r_sma;
				tmp     = multiComp(in[in2], wnk[nk]);
				in[in2] = subComp(in[in1], tmp);
				in[in1] = addComp(in[in1], tmp);
			}
		}
		r_big *= 2;
		r_sma /= 2;
	}
	end = clock();
	printf("計算時間：%lf[s]\n", (double)(end - start) / CLOCKS_PER_SEC);
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

// スペクトル
void ampSpectrum(complex_t Xk[], int N){
	double culc[MAX_SIZE] = {0};
	int i;
	FILE* fp;

	fp = wfileopen();
	printf("スペクトルをファイルに書き込みます。\n");
	for ( i = 0; i < N; i++) {
		culc[i] = sqrt(Xk[i].re * Xk[i].re + Xk[i].im * Xk[i].im);
	}
	for ( i = 0; i < N; i++) {
		fprintf(fp, "%lf\n", culc[i]);
	}
	fclose(fp);
}
