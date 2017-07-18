#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int i;
int lines = 0;
double data1[701], data2[701], data3[71];
double Rxy[701], Rxx[71];

double IP(double[], double[], int);
double CCF(double[], double[], int, int);
void showResult(void);
void writeCrossCorrelationCoefficient(void);
void writeAutocorrelationCoefficient(void);


int main()
{
	/*相互相関係数を求める。*/
	writeCrossCorrelationCoefficient();
	/*事故相互相関係数を求める。*/
	writeAutocorrelationCoefficient();
	/*結果を表示する。*/
	showResult();
	return 0;
}

double IP(double vector1[], double vector2[], int n){
	int i;
	double ip = 0.0;
	for(i = 0; i < n; i++) ip += vector1[i] * vector2[i];
	return ip;
}

double CCF(double data1[], double data2[], int m, int n){
	double sum = 0.0;
	int i;
	for(i = 0; i < n; i++, m++){
		sum += data1[i] * data2[m];
		if(m >= n - 1) break;
	}
	return sum / n;
}

void showResult() {
	printf("相互相関係数：\n");
	printf("Rxy[0]   = %f\n",Rxy[0]);
	printf("Rxy[100] = %f\n",Rxy[100]);
	printf("Rxy[200] = %f\n",Rxy[200]);
	printf("Rxy[300] = %f\n",Rxy[300]);
	printf("Rxy[400] = %f\n",Rxy[400]);
	printf("Rxy[500] = %f\n",Rxy[500]);
	printf("Rxy[600] = %f\n",Rxy[600]);
	printf("Rxy[700] = %f\n\n",Rxy[700]);
	printf("自己相関関数：\n");
	printf("Rxx[0]   = %f\n",Rxx[0]);
	printf("Rxx[10]  = %f\n",Rxx[10]);
	printf("Rxx[20]  = %f\n",Rxx[20]);
	printf("Rxx[30]  = %f\n",Rxx[30]);
	printf("Rxx[40]  = %f\n",Rxx[40]);
	printf("Rxx[50]  = %f\n",Rxx[50]);
	printf("Rxx[60]  = %f\n",Rxx[60]);
	printf("Rxx[70]  = %f\n",Rxx[70]);
}

void writeCrossCorrelationCoefficient(){
	FILE* fp;
	char tmp[252];

	// データを読み込む。
	fp = fopen("wdata1.txt","r");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	while(fgets(tmp, 252, fp) != NULL){
		data1[lines] = atof(tmp);
		lines++;
	}
	fclose(fp);
	lines = 0;

	fp = fopen("wdata2.txt", "r");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	while(fgets(tmp, 252, fp) != NULL){
		data2[lines] = atof(tmp);
		lines++;
	}
	fclose(fp);

	// CCFを求める。
	for(i = 0; i < lines; i++){
		Rxy[i] = CCF(data1, data2, i, lines);
	}

	// データをtxtファイルに出力する。
	fp = fopen("1-4data1.txt", "w");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	for(i = 0; i < lines; i++) fprintf(fp, "%lf\n",Rxy[i]);
	fclose(fp);
	lines = 0;
}

void writeAutocorrelationCoefficient(){
	FILE* fp;
	char tmp[252];

	// データを読み込む。
	fp = fopen("data3.txt", "r");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	while(fgets(tmp, 252, fp) != NULL){
		data3[lines] = atof(tmp);
		lines++;
	}
	fclose(fp);

	// CCFを求める。
	for(i = 0; i < lines; i++){
		Rxx[i] = CCF(data3, data3, i, lines);
	}

	// データをtxtファイルに出力する。
	fp = fopen("1-4data2.txt", "w");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	for(i = 0; i < lines; i++) fprintf(fp,"%lf\n", Rxx[i]);
	fclose(fp);
	lines = 0;
}
