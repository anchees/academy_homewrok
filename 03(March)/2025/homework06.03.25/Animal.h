#pragma once
#include <iostream>
#include <string>
using namespace std;

class Animal {
public:
    virtual void Speak() const {
        cout << "Звук животного." << endl;
    }

    virtual string GetType() const {
        return "Животное";
    }

    virtual ~Animal() {}
};
