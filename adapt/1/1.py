import numpy as np
import glob

dataPath = 'data/'

# ファイル読み込み
def loadFile(fileName):
    return np.loadtxt(dataPath+fileName)


# ファイル書き込み
def writeFile(data):
    np.savetxt('result.csv', data.transpose())


# FFT
def calcFFT(data):
    N = 4
    print('N = {0}'.format(N))
    return np.fft.fft(data[0:0+N])


# DFT
def calcDFT(data, N):
    print('N = {0}'.format(N))
    X = np.zeros((N,), dtype=np.complex)
    for i in range(N):
        sr, si = 0, 0
        for k in range(N):
            wr = np.cos(2 * np.pi * k * i / N)
            wi = - np.sin(2 * np.pi * k * i / N)
            sr = sr + data[k] * wr
            si = si + data[k] * wi
        X[i] = sr + si * 1.j
    return X


# 振幅スペクトルの計算
def calcPowerSpectrum(data):
    return np.abs(data)


def main():
    # testData1, 2の検証
    testData1 = np.array([0, 1, 0, -2])
    testData2 = np.array([-1, 0, -1, -2])
    print(calcDFT(testData1, 4))
    print(calcDFT(testData2, 4))

    # 読み込んだファイルをDFT，振幅スペクトルを求める
    writeFile(calcPowerSpectrum(calcDFT(loadFile('sinwave.txt'), 100)))

if __name__ == '__main__':
    main()
