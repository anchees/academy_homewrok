#ifndef SINGLY_LIST_H
#define SINGLY_LIST_H

#include <iostream>
using namespace std;

class SinglyNode {
public:
    double data;
    SinglyNode* next;

    SinglyNode(double value) : data(value), next(nullptr) {}
};

class SinglyList {
private:
    SinglyNode* head;

public:
    SinglyList() : head(nullptr) {}
    ~SinglyList();

    void Add(double value);
    void Print();
};

SinglyList::~SinglyList() {
    SinglyNode* current = head;
    while (current) {
        SinglyNode* temp = current;
        current = current->next;
        delete temp;
    }
}

void SinglyList::Add(double value) {
    SinglyNode* newNode = new SinglyNode(value);
    if (!head) {
        head = newNode;
    } else {
        SinglyNode* current = head;
        while (current->next)
            current = current->next;
        current->next = newNode;
    }
}

void SinglyList::Print() {
    SinglyNode* current = head;
    while (current) {
        cout << current->data << " -> ";
        current = current->next;
    }
    cout << "null" << endl;
}

#endif
