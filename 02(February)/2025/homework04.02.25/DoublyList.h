#ifndef DOUBLY_LIST_H
#define DOUBLY_LIST_H

#include <iostream>
using namespace std;

class DoublyNode {
public:
    double data;
    DoublyNode* prev;
    DoublyNode* next;

    DoublyNode(double value) : data(value), prev(nullptr), next(nullptr) {}
};

class DoublyList {
private:
    DoublyNode* head;
    DoublyNode* tail;

public:
    DoublyList() : head(nullptr), tail(nullptr) {}
    ~DoublyList();

    void Add(double value);
    void PrintForward();
};

DoublyList::~DoublyList() {
    DoublyNode* current = head;
    while (current) {
        DoublyNode* temp = current;
        current = current->next;
        delete temp;
    }
}

void DoublyList::Add(double value) {
    DoublyNode* newNode = new DoublyNode(value);
    if (!tail) {
        head = tail = newNode;
    } else {
        tail->next = newNode;
        newNode->prev = tail;
        tail = newNode;
    }
}

void DoublyList::PrintForward() {
    DoublyNode* current = head;
    while (current) {
        cout << current->data << " <-> ";
        current = current->next;
    }
    cout << "null" << endl;
}

#endif
