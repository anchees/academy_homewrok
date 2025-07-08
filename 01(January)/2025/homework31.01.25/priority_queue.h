#ifndef PRIORITY_QUEUE_H
#define PRIORITY_QUEUE_H

#include <iostream>
using namespace std;

class PriorityQueue {
private:
    struct Element {
        string value;
        int priority;
    };

    Element* data;
    int capacity;
    int count;

public:
    PriorityQueue(int size);
    ~PriorityQueue();

    bool isEmpty();
    bool isFull();

    void push(string val, int prio);
    void pop();
    string top();
    void print();
};

#endif
