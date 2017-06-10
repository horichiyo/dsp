#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int  lines = 0;
double ip1, ip2, ip3, ip4, ip5, ip6;
double Norm1, Norm2, Norm3, Norm4, Norm5, Norm6, Norm7, Norm8;
double H16man[86], H16woman[86], H16population[86], S24population[86];
double average1, average2, average3, average4;
double data1[86], data2[86], data3[86], data4[86];

double average(double[], int);
void subtraction(double[], double[], double, int);
double IP(double[], double[], int);
double norm(double[], int);
void showResult(void);
void calcAverage(void);
void calcNorm(void);
void calcIP(void);
void calcSubracttion(void);
void readData(void);
FILE* readFile(char[]);

int main()
{
	/*データを読み込む*/
	readData();
	/*平均を求める*/
	calcAverage();
	/*平均を引く*/
	calcSubracttion();
	/*内積を求める*/
	calcIP();
	/*大きさを求める*/
	calcNorm();
	/*表示*/
	showResult();
	return 0;
}


double average(double num[], int n){
	int i;
	double sum = 0.0;
	for(i = 0; i < n; i++) sum += num[i];
	return sum / n;
}

void subtraction(double data1[], double data2[], double average, int n){ /*元データ|代入*/
	int i;
	for(i = 0; i < n; i++) data2[i] = data1[i] - average;
}

double IP(double vector_1[], double vector_2[], int n){
	int i;
	double ip = 0.0;
	for(i = 0; i < n; i++) ip += vector_1[i] * vector_2[i];
	return ip;
}

double norm(double vector[], int n){
	int i;
	double Norm = 0.0;
	for(i = 0; i < n; i++) Norm += vector[i] * vector[i];
	return sqrt(Norm);
}

void showResult(){
	printf("相関関数結果:\n");
	printf("H16男性・女性 = %.3f\n", ip1 / (Norm1 * Norm2));
	printf("H16男性・総人口 = %.3f\n", ip2 / (Norm1 * Norm3));
	printf("H16男性・S24 = %.3f\n\n", ip3 / (Norm1 * Norm4));
	printf("直流成分未除去の場合:\n");
	printf("H16男性・女性 = %.3f\n", ip4 / (Norm5 * Norm6));
	printf("H16男性・総人口 = %.3f\n", ip5 / (Norm5 * Norm7));
	printf("H16男性・S24 = %.3f\n\n", ip6 / (Norm5 * Norm8));
}

void calcAverage(){
	average1 = average(H16man, lines);
	average2 = average(H16woman, lines);
	average3 = average(H16population, lines);
	average4 = average(S24population, lines);
}

void calcIP(){
	ip1 = IP(data1, data2, lines);
	ip2 = IP(data1, data3, lines);
	ip3 = IP(data1, data4, lines);
	ip4 = IP(H16man, H16woman, lines);
	ip5 = IP(H16man, H16population, lines);
	ip6 = IP(H16man, S24population, lines);
}

void calcNorm(){
	Norm1 = norm(data1, lines);
	Norm2 = norm(data2, lines);
	Norm3 = norm(data3, lines);
	Norm4 = norm(data4, lines);
	Norm5 = norm(H16man, lines);
	Norm6 = norm(H16woman, lines);
	Norm7 = norm(H16population, lines);
	Norm8 = norm(S24population, lines);

}

void calcSubracttion(){
	subtraction(H16man, data1, average1, lines);
	subtraction(H16woman, data2, average2, lines);
	subtraction(H16population, data3, average3, lines);
	subtraction(S24population, data4, average4, lines);
}

FILE* readFile(char filename[]){
	FILE* fp;

	fp = fopen(filename, "r");
	if(fp == NULL){
		printf("can't open a file.\n");
		exit(1);
	}
	return fp;
}

void readData() {
	FILE *fp;
	char tmp[252];

	fp = readFile("rdata1.txt");
	while(fgets(tmp, 252, fp) != NULL){
		H16man[lines] = atoi(tmp);
		lines++;
	}
	fclose(fp);
	lines = 0;

	fp = readFile("rdata2.txt");
	while(fgets(tmp, 252, fp) != NULL){
		H16woman[lines] = atoi(tmp);
		lines++;
	}
	fclose(fp);
	lines = 0;

	fp = readFile("rdata3.txt");
	while(fgets(tmp, 252, fp) != NULL){
		H16population[lines] = atoi(tmp);
		lines++;
	}
	fclose(fp);
	lines = 0;

	fp = readFile("rdata4.txt");
	while(fgets(tmp, 252, fp) != NULL){
		S24population[lines] = atoi(tmp);
		lines++;
	}
	fclose(fp);
}
