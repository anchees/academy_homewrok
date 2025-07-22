#pragma once
#include <iostream>
#include <stdexcept>
using namespace std;

class DoubleList {
private:
    struct Node {
        int value;
        Node* next;
        Node* prev;
        Node(int val) : value(val), next(nullptr), prev(nullptr) {}
    };

    Node* head;
    Node* tail;

public:
    DoubleList() : head(nullptr), tail(nullptr) {}

    ~DoubleList() {
        while (head) {
            Node* temp = head;
            head = head->next;
            delete temp;
        }
    }

    void push_back(int val) {
        Node* node = new Node(val);
        if (!head) {
            head = tail = node;
        } else {
            tail->next = node;
            node->prev = tail;
            tail = node;
        }
    }

    void pop_back() {
        if (!tail)
            throw runtime_error("Нельзя удалить из пустого списка");

        Node* temp = tail;
        if (tail == head) {
            head = tail = nullptr;
        } else {
            tail = tail->prev;
            tail->next = nullptr;
        }
        delete temp;
    }

    void print_forward() const {
        Node* curr = head;
        while (curr) {
            cout << curr->value << " ";
            curr = curr->next;
        }
        cout << endl;
    }
};
