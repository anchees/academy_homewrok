#ifndef STACK_H
#define STACK_H

#include <iostream>
using namespace std;

class Stack {
private:
    int* data;      
    int size;       
    int topIndex;   

public:
    Stack(int size = 10) {
        this->size = size;
        data = new int[size];
        topIndex = -1;
    }

    ~Stack() {
        delete[] data;
    }

    void push(int value) {
        if (isFull()) {
            cout << "Стек переполнен!" << endl;
            return;
        }
        data[++topIndex] = value;
    }

    void pop() {
        if (isEmpty()) {
            cout << "Стек пуст!" << endl;
            return;
        }
        --topIndex;
    }

    int top() const {
        if (isEmpty()) {
            cout << "Стек пуст!" << endl;
            return -1;
        }
        return data[topIndex];
    }

    bool isEmpty() const {
        return topIndex == -1;
    }

    bool isFull() const {
        return topIndex == size - 1;
    }

    void print() const {
        cout << "Содержимое стека: ";
        for (int i = 0; i <= topIndex; ++i)
            cout << data[i] << " ";
        cout << endl;
    }

    int getCount() const {
        return topIndex + 1;
    }

    int getCapacity() const {
        return size;
    }
};
#endif
