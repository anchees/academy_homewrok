#include "Student.h"

int main() {
    setlocale(LC_ALL, "Russian");

    Student s1("Иван Иванов", 20, 4.5);
    s1.save_to_file("student.txt");

    Student s2;
    s2.load_from_file("student.txt");
    s2.display();

    return 0;
}
