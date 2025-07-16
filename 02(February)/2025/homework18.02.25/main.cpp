// create_file.cpp
#include <iostream>
#include <fstream>
#include <filesystem>
#include <string>

using namespace std;
namespace fs = filesystem;

string get_unique_filename(const string& original_name) {
    fs::path file_path(original_name);
    string stem = file_path.stem().string();           
    string extension = file_path.extension().string();
    string parent_path = file_path.parent_path().string();

    string unique_name = file_path.filename().string();
    int counter = 2;

    while (fs::exists(parent_path.empty() ? unique_name : parent_path + "/" + unique_name)) {
        unique_name = stem + " (" + to_string(counter) + ")" + extension;
        ++counter;
    }

    return parent_path.empty() ? unique_name : parent_path + "/" + unique_name;
}


void Create_File(const string& filename) {
    string unique_filename = get_unique_filename(filename);
    ofstream file(unique_filename);
}


int main() {
    string filename;
    cout << "Введите имя файла: ";
    getline(cin, filename);

    Create_File(filename);
    return 0;
}
