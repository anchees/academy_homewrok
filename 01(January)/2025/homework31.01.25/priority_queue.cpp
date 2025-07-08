#include "priority_queue.h"

PriorityQueue::PriorityQueue(int size) {
    capacity = size;
    data = new Element[capacity];
    count = 0;
}

PriorityQueue::~PriorityQueue() {
    delete[] data;
}

bool PriorityQueue::isEmpty() {
    return count == 0;
}

bool PriorityQueue::isFull() {
    return count == capacity;
}

void PriorityQueue::push(string val, int prio) {
    if (isFull()) {
        cout << "Очередь переполнена!" << endl;
        return;
    }
    int i = count - 1;
    while (i >= 0 && data[i].priority < prio) {
        data[i + 1] = data[i];
        i--;
    }
    data[i + 1].value = val;
    data[i + 1].priority = prio;
    count++;
}

void PriorityQueue::pop() {
    if (isEmpty()) {
        cout << "Очередь пуста!" << endl;
        return;
    }
    for (int i = 0; i < count - 1; i++) {
        data[i] = data[i + 1];
    }
    count--;
}

string PriorityQueue::top() {
    if (isEmpty()) {
        cout << "Очередь пуста!" << endl;
        return "";
    }
    return data[0].value;
}

void PriorityQueue::print() {
    cout << "Очередь (значение:приоритет): ";
    for (int i = 0; i < count; i++) {
        cout << data[i].value << ":" << data[i].priority << " ";
    }
    cout << endl;
}
