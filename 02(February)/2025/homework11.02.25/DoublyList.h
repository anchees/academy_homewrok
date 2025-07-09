#ifndef DOUBLY_LIST_H
#define DOUBLY_LIST_H

#include <iostream>
#include <initializer_list>
using namespace std;

template <typename T>
class DoublyList {
private:
    struct Node {
        T data;
        Node* prev;
        Node* next;
        Node(const T& value) : data(value), prev(nullptr), next(nullptr) {}
    };

    Node* head;
    Node* tail;

public:
    DoublyList() : head(nullptr), tail(nullptr) {}

    DoublyList(initializer_list<T> list) : head(nullptr), tail(nullptr) {
        for (const T& value : list) {
            Add(value);
        }
    }

    ~DoublyList() {
        Clear();
    }

    DoublyList(DoublyList&& other) noexcept : head(other.head), tail(other.tail) {
        other.head = nullptr;
        other.tail = nullptr;
    }

    DoublyList& operator=(DoublyList&& other) noexcept {
        if (this != &other) {
            Clear();
            head = other.head;
            tail = other.tail;
            other.head = nullptr;
            other.tail = nullptr;
        }
        return *this;
    }

    void Add(const T& value) {
        Node* newNode = new Node(value);
        if (!tail) {
            head = tail = newNode;
        } else {
            tail->next = newNode;
            newNode->prev = tail;
            tail = newNode;
        }
    }
    
    void PrintForward() const {
        Node* current = head;
        while (current) {
            cout << current->data << " <-> ";
            current = current->next;
        }
        cout << "null" << endl;
    }

private:
    void Clear() {
        Node* current = head;
        while (current) {
            Node* temp = current;
            current = current->next;
            delete temp;
        }
        head = tail = nullptr;
    }
};

#endif
