#ifndef STATARRAY_H
#define STATARRAY_H

#include <iostream>
using namespace std;

template<typename T, int SIZE>
class StatArray {
private:
    T data[SIZE];

public:
    T& operator[](int index) {
        return data[index];
    }

    const T& operator[](int index) const {
        return data[index];
    }
};

#endif 
