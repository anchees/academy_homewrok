#pragma once
#include <string>

using namespace std;

class Student {
private:
    string name;
    int age;
    double gpa;

public:
    Student() = default;
    Student(const string& name, int age, double gpa);

    void display() const;

    void save_to_file(const string& filename) const;
    void load_from_file(const string& filename);
};
