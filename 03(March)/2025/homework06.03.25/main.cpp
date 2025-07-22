#include <iostream>
#include "Tiger.h"
#include "Lion.h"
#include "Ligr.h"
using namespace std;

int main() {
    Tiger tiger;
    Lion lion;
    Ligr ligr;

    Animal* animals[] = { &tiger, &lion, &ligr };

    cout << "Звуки животных:\n";
    for (Animal* a : animals) {
        cout << a->GetType() << ": ";
        a->Speak();
    }

    cout << "\nЛев рычит 3 раза:\n";
    lion.Speak(3);

    cout << "\nТигр рычит:\n";
    tiger.Speak();

    cout << "\nЛигр рычит 2 раза:\n";
    ligr.Speak(2);

    return 0;
}
