#ifndef MATRIX_H
#define MATRIX_H

#include "StatArray.h"
#include <iostream>
#include <cstdlib> 
#include <ctime>
using namespace std;

template<typename T, int ROWS, int COLS>
class Matrix {
private:
    StatArray<StatArray<T, COLS>, ROWS> data;

public:
    void input() {
        cout << "Введите элементы матрицы " << ROWS << "x" << COLS << ":\n";
        for (int i = 0; i < ROWS; ++i)
            for (int j = 0; j < COLS; ++j) {
                cout << "[" << i << "][" << j << "]: ";
                cin >> data[i][j];
            }
    }

    void fillRandom(T minVal = 0, T maxVal = 10) {
        srand(time(0));
        for (int i = 0; i < ROWS; ++i)
            for (int j = 0; j < COLS; ++j)
                data[i][j] = minVal + rand() % (maxVal - minVal + 1);
    }

    void print() const {
        for (int i = 0; i < ROWS; ++i) {
            for (int j = 0; j < COLS; ++j)
                cout << data[i][j] << "\t";
            cout << endl;
        }
    }

    Matrix operator+(const Matrix& other) const {
        Matrix result;
        for (int i = 0; i < ROWS; ++i)
            for (int j = 0; j < COLS; ++j)
                result.data[i][j] = data[i][j] + other.data[i][j];
        return result;
    }

    Matrix operator-(const Matrix& other) const {
        Matrix result;
        for (int i = 0; i < ROWS; ++i)
            for (int j = 0; j < COLS; ++j)
                result.data[i][j] = data[i][j] - other.data[i][j];
        return result;
    }

    Matrix operator*(const Matrix& other) const {
        Matrix result;
        for (int i = 0; i < ROWS; ++i)
            for (int j = 0; j < COLS; ++j)
                result.data[i][j] = data[i][j] * other.data[i][j];
        return result;
    }
};
#endif
