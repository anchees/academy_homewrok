#pragma once
#include "Animal.h"
using namespace std;

class Cat : public virtual Animal {
public:
    void Speak() const override {
        cout << "Мяу" << endl;
    }

    string GetType() const override {
        return "Кошка";
    }
};
