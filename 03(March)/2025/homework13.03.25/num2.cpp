#include <iostream>
#include <string>
using namespace std;

int main() {
    string text;
    string pattern;

    cout << "Введите текст: ";
    getline(cin, text);

    cout << "Введите подстроку для поиска: ";
    getline(cin, pattern);

    size_t pos = text.find(pattern);

    if (pos != string::npos)
        cout << "Подстрока найдена на позиции: " << pos << endl;
    else
        cout << "Подстрока не найдена." << endl;

    return 0;
}
