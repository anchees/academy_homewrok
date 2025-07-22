#pragma once
#include <iostream>
#include <stdexcept>
using namespace std;

class DynArray {
private:
    int* data;
    int size;

public:
    DynArray(int s) : size(s) {
        if (size <= 0)
            throw invalid_argument("Размер массива должен быть положительным");

        try {
            data = new int[size];
        }
        catch (bad_alloc&) {
            throw runtime_error("Ошибка выделения памяти");
        }

        for (int i = 0; i < size; i++)
            data[i] = 0;
    }

    ~DynArray() {
        delete[] data;
    }

    int& operator[](int index) {
        if (index < 0 || index >= size)
            throw out_of_range("Выход за границы массива");
        return data[index];
    }

    int getSize() const {
        return size;
    }
};
