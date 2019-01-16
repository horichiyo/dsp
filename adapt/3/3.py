import numpy as np
import math


fs = 10000 # サンプリング周波数
fc = 2000  # カットオフ周波数
N=7        # 入力点数


# 変数
T = 1.0 / fs
omega_d = 2.0*math.pi * fc
omega_a = 2.0/T * math.tan(omega_d*T/2.0)
alpha = math.tan(omega_d*T / 2.0)
a1 = (alpha-1) / (alpha+1)
b0 = b1 = alpha / (alpha+1)

print(omega_a)
print(alpha)
print(a1)
print(b0)
print('--')

# インパルス入力
input = np.zeros(N)
input[0] = 1

# ローパスフィルタ
x1 = y1 = 0.0
output = np.array([])
for j in range(N):
	y0 = b0*input[j] + b1*x1 - a1*y1
	print(y0)
	x1 = input[j]
	y1 = y0
	output = np.append(output, y0)


print(output)
