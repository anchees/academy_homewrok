#include "Student.h"
#include <fstream>
#include <iostream>

Student::Student(const string& name, int age, double gpa)
    : name(name), age(age), gpa(gpa) {}

void Student::display() const {
    cout << "Имя: " << name << ", Возраст: " << age << ", Средний балл: " << gpa << endl;
}

void Student::save_to_file(const string& filename) const {
    ofstream out(filename);
    if (!out) {
        cerr << "Ошибка открытия файла для записи: " << filename << endl;
        return;
    }
    out << name << endl << age << endl << gpa << endl;
    out.close();
}

void Student::load_from_file(const string& filename) {
    ifstream in(filename);
    if (!in) {
        cerr << "Ошибка открытия файла для чтения: " << filename << endl;
        return;
    }
    getline(in, name);
    in >> age >> gpa;
    in.close();
}
