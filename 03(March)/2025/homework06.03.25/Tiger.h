#pragma once
#include "Cat.h"
using namespace std;

class Tiger : public virtual Cat {
public:
    void Speak() const override {
        cout << "Ррррааа (Тигр)" << endl;
    }

    string GetType() const override {
        return "Тигр";
    }
};
