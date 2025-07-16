#include <iostream>
#include <fstream>
#include <filesystem>
#include <string>

using namespace std;
namespace fs = filesystem;

fs::path get_unique_path(const fs::path& original_path) {
    if (!fs::exists(original_path))
        return original_path;

    fs::path parent = original_path.parent_path();
    string stem = original_path.stem().string();
    string extension = original_path.has_extension() ? original_path.extension().string() : "";
    int counter = 2;

    fs::path new_path;
    do {
        new_path = parent / (stem + " (" + to_string(counter++) + ")" + extension);
    } while (fs::exists(new_path));

    return new_path;
}

void Create_File(const string& filename) {
    fs::path file_path = get_unique_path(filename);
    ofstream file(file_path);
    if (file.is_open()) {
        cout << "Файл успешнно создан: " << file_path << endl;
        file.close();
    } else {
        cerr << "Не удалось создать файл: " << file_path << endl;
    }
}

void Rename_File(const string& old_name, const string& new_name) {
    fs::path old_path(old_name);
    if (!fs::exists(old_path)) {
        cerr << "Файл не найден: " << old_name << endl;
        return;
    }

    fs::path new_path = get_unique_path(new_name);
    fs::rename(old_path, new_path);
    cout << "Файл переименован: " << new_path << endl;
}

void Move_File(const string& source, const string& destination_dir) {
    fs::path src_path(source);
    fs::path dest_path = fs::path(destination_dir) / src_path.filename();
    dest_path = get_unique_path(dest_path);

    if (!fs::exists(src_path)) {
        cerr << "Файл не найден: " << source << endl;
        return;
    }

    if (!fs::exists(destination_dir)) {
        cerr << "Папка не найденга: " << destination_dir << endl;
        return;
    }

    fs::rename(src_path, dest_path);
    cout << "Файл перемещён: " << dest_path << endl;
}

void Rename_Directory(const string& old_name, const string& new_name) {
    fs::path old_path(old_name);
    if (!fs::exists(old_path) || !fs::is_directory(old_path)) {
        cerr << "Папка не найдена: " << old_name << endl;
        return;
    }

    fs::path new_path = get_unique_path(new_name);
    fs::rename(old_path, new_path);
    cout << "Папка переименована: " << new_path << endl;
}

void Move_Directory(const string& source, const string& destination_dir) {
    fs::path src_path(source);
    if (!fs::exists(src_path) || !fs::is_directory(src_path)) {
        cerr << "Папка не найдена: " << source << endl;
        return;
    }

    if (!fs::exists(destination_dir)) {
        cerr << "Папка не существует: " << destination_dir << endl;
        return;
    }

    fs::path dest_path = fs::path(destination_dir) / src_path.filename();
    dest_path = get_unique_path(dest_path);

    fs::rename(src_path, dest_path);
    cout << "Папка перемещена: " << dest_path << endl;
}

int main() {
    Create_File("Документ.txt");
    Rename_File("Документ.txt", "Тест.txt");
    Move_File("Тест.txt", "backup");

    Rename_Directory("data", "архив");
    Move_Directory("архив", "backup");

    return 0;
}
