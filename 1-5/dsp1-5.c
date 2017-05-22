#include <stdio.h>
#include <stdlib.h>
#define _USE_MATH_DEFINES
#include <math.h>

typedef struct {
	double re;
	double im;
} complex_t;

void DF(complex_t[], complex_t[], int, int, int);
double amp_spectrum(complex_t);
double phase_spectrum(complex_t);
void windows(complex_t[], int);
FILE* wfileopen(void);
FILE* rfileopen(void);

int main()
{
	int selectDFT = 0;
	int selectOutput = 0;
	int window  = 0;
	int flag    = 0;
	int fileout = 0;
	int a,b,i;
	int count;
	double amp[10000] = {0};
	double phase[10000] = {0};
	char rfilename[256];
	char wfilename[256];
	FILE* fp;
	FILE* fr;
	complex_t xn[10000] = {0};
	complex_t Xk[10000] = {0};

	fr = rfileopen();
	fp = wfileopen();

	// DFT・IDFTの選択
	while(flag == 0){
		printf("DFT-->1 IDFT-->2\n--> ");
		scanf("%d", &selectDFT);
		if(selectDFT == 1 || selectDFT == 2){
			flag = 1;
		}else{
			printf("1か２を入力して下さい。\n");
		}
	}

	// 点数の選択
	printf("点数\n--> ");
	scanf("%d", &count);

	// 窓関数を使用するか
	if(selectDFT){
		flag = 0;
		while(flag == 0){
			printf("窓関数を使用：1　利用しない：0\n--> ");
			scanf("%d", &window);
			if(window == 1 || window == 0){
				flag = 1;
			}else{
				printf("1か０を入力して下さい。\n");
			}
		}

		flag = 0;
		if(window){
			while(flag == 0){
				printf("ファイルを出力して終了：1　続ける：0\n--> ");
				scanf("%d", &fileout);
				if(fileout == 1 || fileout == 0){
					flag = 1;
				}else{
					printf("1か０を入力して下さい。\n");
				}
			}
			if(fileout){
				for(i = 0; i < count; i++) fscanf(fr,"%lf",&xn[i].re);
				fclose(fr);
				windows(xn, count);
				for (i = 0; i < count; i++) fprintf(fp, "%f\n", xn[i].re);
				fclose(fp);
				exit(1);
			}

		}
	}

	// 何を出力するかチェック
	flag = 0;
	while(flag == 0){
		printf("振幅スペクトル[dB]，位相スペクトル[deg]を出力：1\n");
		printf("DFT結果を出力：2\n-->");
		scanf("%d", &selectOutput);
		if(selectOutput == 1 || selectOutput == 2){
			flag = 1;
		}else{
			printf("1か２を入力して下さい。\n");
		}
	}

	// 実際に行うところ
	if(selectDFT){//DFT
		a = 1;
		b = 1;
		for(i = 0; i < count; i++) fscanf(fr, "%lf", &xn[i].re);
		fclose(fr);
		if(window) windows(xn, count);
		DF(xn, Xk, count, a, b);
		if(selectOutput == 2){//DFTの出力
			for(i = 0; i < count; i++) fprintf(fp, "%f %f\n", Xk[i].re, Xk[i].im);
		}else{//振幅，位相スペクトルの出力
			for(i = 0; i < count; i++){
				amp[i] = amp_spectrum(Xk[i]);
				phase[i] = phase_spectrum(Xk[i]);
				fprintf(fp, "%f	%f\n", amp[i], phase[i]);
			}
		}
		fclose(fp);
		printf("ファイル書き込み終了。\n");
	}else{//IDFT
		a = -1;
		b = count;
		for(i = 0; i < count; i++) fscanf(fr, "%lf	%lf", &xn[i].re, &xn[i].im);
		fclose(fr);
		DF(xn, Xk, count, a, b);
		if(selectOutput == 2){
			for(i = 0; i < count; i++) fprintf(fp, "%f %f\n", Xk[i].re, Xk[i].im);
		}else{
			for(i = 0; i < count; i++){
				amp[i] = amp_spectrum(Xk[i]);
				phase[i] = phase_spectrum(Xk[i]);
				fprintf(fp, "%f %f\n", amp[i], phase[i]);
			}
		}
		fclose(fp);
		printf("ファイル書き込み終了。\n");
	}
	return 0;
}

/*DFT・IDFT*/
void DF(complex_t xn[], complex_t Xk[], int N, int a, int b){ /*DFT:a=1,b=1  IDFT:a=-1,b=N(N点IDFT)*/
	int n,k;
	for(k = 0; k < N; k++){
		for(n = 0; n < N; n++){
			Xk[k].re += (xn[n].re * cos((2 * M_PI * n * k) / N) + a * xn[n].im * sin((2 * M_PI * n * k) / N)) / b;
			Xk[k].im += (xn[n].im * cos((2 * M_PI * n * k) / N) - a * xn[n].re * sin((2 * M_PI * n * k) / N)) / b;
		}
	}
}

/*振幅スペクトル*/
double amp_spectrum(complex_t Xk){
	double culc = 0.0;
	culc = Xk.re * Xk.re + Xk.im * Xk.im;
	return 20 * log10(fabs(sqrt(culc)));
}

/*位相スペクトル*/
double phase_spectrum(complex_t Xk){
	double culc = 0.0;
	if(fabs(Xk.im) < 1.0e-5 && fabs(Xk.re) < 1.0e-4) return 0;
	culc = atan2(Xk.im, Xk.re);
	return culc;
}

/*窓関数*/
void windows(complex_t xn[], int N){
	int i;
	for(i = 0; i < N; i++) xn[i].re = xn[i].re * (0.54 - 0.46 * cos(2 * M_PI * i / N));
}

/*書き込みモード*/
FILE* wfileopen(){
	FILE* fp;
	char filename[256];
	printf("書き込むファイル名\n--> ");
	scanf("%s", filename);
	fp = fopen(filename, "w");
	if (fp == NULL) {
		printf("can't open file.\n");
		exit(-1);
	}
	return fp;
}

/*読み込みモード*/
FILE* rfileopen(){
	FILE* fr;
	char filename[256];
	printf("読み込むファイル名\n--> ");
	scanf("%s", filename);
	fr = fopen(filename, "r");
	if (fr == NULL) {
		printf("can't open file.\n");
		exit(-1);
	}
	return fr;
}
