#include <stdio.h>   //
#include <stdlib.h>  //
#include <time.h>    //
#include <math.h>    //
//#include <ctype.h>   //
//#include <string.h>  //
//#include <malloc.h>  //
//#include <memory.h>  //

#define N 20		//FIRフィルタ係数の数（未知システム，適応フィルタとも同じとする）
#define step 1		//ステップサイズ（ノイズを考えないので通常1）
#define samp 8000	//サンプル数（サンプルに対するアルゴリズムの繰り返し回数）
#define ave_samp 1	//試行回数
#define rate 20		//ファイル出力サンプル間隔
#define r 20  		//ブロック長(1は学習同定法)

double gauss(void);	//白色ガウス性雑音発生関数
double colored(void);	//有色信号発生関数(1次IIRフィルタ利用)

//配列初期化関数
void init_vector(double *, int);	//ベクトル初期化
void init_matrix(double **, int, int);	//行列初期化

//メイン　
int main()
{
	char wfile[255];
	double **xnr;		//入力信号行列
	double *xn;		//入力信号ベクトル
	double *hn;		//推定システム
	double *wn;		//未知システム
	double **gnr;		//直交化された入力信号
	double *gnorm;		//内積用配列
	double *nee_ave;	//平均用配列(課題では特に必要ない)

	double *hdm;

	double w_power=0,xgnorm,wh,NEE,d,y,e;
	int i,j,m,k,ave,ol;
	FILE *fpo;

	srand((unsigned)time(NULL));

	//配列確保
	if((hn = (double *)	calloc( N, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
	if((wn = (double *)	calloc( N, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
	if((xn = (double *)	calloc( N, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
	if((gnorm = (double *)	calloc( N, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
	if((hdm = (double *)	calloc( r, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}

	if((nee_ave = (double *)	calloc( samp+1, sizeof( double )) ) == NULL ){printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
	if((xnr=(double **)calloc(N, sizeof(double *))) == NULL){ printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
		for(i=0;i<N;i++) if((xnr[i]=(double *)calloc(r, sizeof(double))) == NULL){ printf("領域確保ができません\n");exit(EXIT_FAILURE);};
	if((gnr=(double **)calloc(N, sizeof(double *))) == NULL){ printf("領域確保ができません\n"); exit(EXIT_FAILURE);}
		for(i=0;i<N;i++) if((gnr[i]=(double *)calloc(r, sizeof(double))) == NULL){ printf("領域確保ができません\n");exit(EXIT_FAILURE);};

	printf("ファイル名を入力してください\n");
	scanf("%s",&wfile);
	fpo=fopen(wfile,"w");

	wn[0]=2.1352262415215782;		wn[1]=7.3531023680889801;
	wn[2]=2.1352262415150772;		wn[3]=-0.4377715519333333;
	wn[4]=0.1955904688080233;		wn[5]=-0.1152505302566677;
	wn[6]=0.07919222015968772;		wn[7]=-0.06031997433299987;
	wn[8]=0.04964680644004544;		wn[9]=-0.04353370082452390;
	wn[10]=0.04034426306334975;		wn[11]=-0.03935085341244733;
	wn[12]=0.04034426306473789;		wn[13]=-0.04353370082175066;
	wn[14]=0.04964680644446781;		wn[15]=-0.06031997432660415;
	wn[16]=0.07919222016864548;		wn[17]=-0.1152505302448674;
	wn[18]=0.1955904688246496;		wn[19]=-0.4377715519036359;
	for(i=0;i<N;i++) w_power+=wn[i]*wn[i];//あらかじめノルムの２乗を計算しておく

//ここから平均ループ（課題では特に必要ない）
	for(ave=1;ave<=ave_samp;ave++){

//平均試行のための配列初期化（課題では特に必要ない）
		for(i=0;i<N;i++) hn[i]=0.0;

//ここからサンプル(ブロック)ループ（逐次処理はサンプル毎，ブロック処理はrサンプル毎にフィルタが更新される）
		for(k=0;k<=samp/r;k++){

//入力行列の生成//////////
			//xnの更新
			if(k==0) ol=40;//配列にデータを満たす
			else ol=r;
			for (m=0;m<ol;m++){
				for(i=N-1;i>0;i--)	xn[i]=xn[i-1];//入力ベクトルを生成
				// xn[0]=gauss();		//白色		＜−−入力信号を選択
				xn[0]=colored();	//有色
				//xnr行列の更新
				for(i=r-1;i>0;i--) for(j=0;j<N;j++) xnr[j][i]=xnr[j][i-1];//行列の要素をずらす
				for(i=0;i<N;i++) xnr[i][0]=xn[i];//行列に上で生成したベクトルを挿入する
			}
//ここからアルゴリズム本体の計算////////////////
//行列・ベクトルの初期化//////////
			init_vector(gnorm, r);
			init_vector(hdm, r);
			init_matrix(gnr,N,r);

//アルゴリズム部分//////////
			for(m=0;m<r;m++){//1回のフィルタ更新で，r回繰り返される

//所望信号の計算
				d=0.0;
				for(i=0;i<N;i++) d += xnr[i][m]*wn[i];


//sumの繰り返し部分ベクトルの直交化
				for(i=0;i<m;i++){
					xgnorm=0.0;
					for(j=0;j<N;j++) xgnorm+=xnr[j][m]*gnr[j][i];
					for(j=0;j<N;j++) gnr[j][m]+=xgnorm/gnorm[i]*gnr[j][i];
					hdm[m]+= xgnorm/gnorm[i]*hdm[i];//ここに(d^)の計算一部挿入

				}

				for(i=0;i<N;i++) gnr[i][m] = xnr[i][m]-gnr[i][m];
				hdm[m]= d - hdm[m];//ここで(d^)の計算挿入

				y=0.0;
				for(i=0;i<N;i++) y += gnr[i][m]*hn[i];
				e=hdm[m]-y;

				for(i=0;i<N;i++) gnorm[m] += gnr[i][m]*gnr[i][m];
				for(i=0;i<N;i++) hn[i] += step*gnr[i][m]/gnorm[m]*e;
			}
//評価量（正規化推定誤差:NEE）/////////////////////////////////
			wh=0.0;
			for(i=0;i<N;i++) wh += (wn[i] - hn[i])*(wn[i] - hn[i]);
			NEE = 10 * log10(wh / w_power);
			nee_ave[k] += NEE;

			//NEE平均の計算
			if(ave==ave_samp){
				nee_ave[k] /= ave_samp;
				//一定間隔でのファイル出力
				if((k*r % rate)==0)	fprintf(fpo,"%7d\t%lf\n",k*r, nee_ave[k]);
				if((k*r % rate)==0)	printf("%7d\t%lf\n",k*r, nee_ave[k]);
			}
		}//ブロック番号ループ k
	}//平均ループ ave

	getchar();
	getchar();

	free(xn);free(hn);free(wn);free(gnorm);	free(nee_ave);
       	for(i=0;i<N;i++){
		free(xnr[i]);free(gnr[i]);
	}
	free(xnr);free(gnr);

	fclose(fpo);
	return 0;
}

////////////////////////////////////////////////////////////////////
//白色ガウス性雑音作成関数
double gauss(void)//正規乱数（ボックスミュラー法を利用）
{
	double x1,x2,gauss1,gauss2,sd=1.0;//sdは分散
	double pi=3.1415926535897932384626433832795;

	x1=(double)rand()/(RAND_MAX);//0-1の一様乱数１
		while(x1 == 0.0){
			x1=(double)rand()/(RAND_MAX);
	}
	x2=(double)rand()/(RAND_MAX);//0-1の一様乱数２
	gauss1=sqrt(-2.0*log(x1))*cos(2.0*pi*x2)*sqrt(sd);
	gauss2=sqrt(-2.0*log(x1))*sin(2.0*pi*x2)*sqrt(sd);//gauss1と独立な正規乱数
	return gauss1;
}

////////////////////////////////////////////////////////////////////
//１次IIRフィルタによる有色信号作成関数
double colored(void)
{
	double color;
	static double oldcolor=0.0;
	color = gauss() + 0.95*oldcolor;
	oldcolor = color;
	return color;
}

//ベクトルの初期化///////////////////////////////
void init_vector(double *a, int b)
{
	int i;
	for(i=0;i<b;i++)	a[i]=0.0;
}

//行列の初期化///////////////////////////////
void init_matrix(double **a, int b, int c)
{
	int i,j;
	for(i=0;i<b;i++) for(j=0;j<c;j++) a[i][j]=0.0;
}
