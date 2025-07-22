#pragma once
#include "Lion.h"
#include "Tiger.h"
using namespace std;

class Ligr : public Lion, public Tiger {
public:
    void Speak() const override {
        cout << "Лигр издаёт гибридный рык!" << endl;
    }

    void Speak(int times) const override {
        for (int i = 0; i < times; ++i)
            cout << "Лигр рычит!" << endl;
    }

    string GetType() const override {
        return "Лигр";
    }
};
