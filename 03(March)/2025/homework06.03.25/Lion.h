#pragma once
#include "Cat.h"
using namespace std;

class Lion : public virtual Cat {
public:
    void Speak() const override {
        cout << "Ррррааа (Лев)" << endl;
    }

    virtual void Speak(int times) const {
        for (int i = 0; i < times; ++i)
            cout << "Лев рычит!" << endl;
    }

    string GetType() const override {
        return "Лев";
    }
};
