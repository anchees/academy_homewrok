#include <iostream>
#include <fstream>
#include <string>
using namespace std;

class Document {
public:
    virtual void Create() = 0;
    virtual void Open(string filename) = 0;
    virtual void Save() = 0;
    virtual void SaveAs(string newName) = 0;
    virtual void Print() = 0;
    virtual void Close() = 0;
    virtual ~Document() {}
};

class TextDocument : public Document {
private:
    string filename;
    string text;
public:
    void Create() override {
        text = "";
        filename = "new.txt";
    }

    void Open(string fname) override {
        filename = fname;
        ifstream fin(fname);
        if (fin.is_open()) {
            text = "";
            string line;
            while (getline(fin, line)) {
                text += line + "\n";
            }
        } else {
            cout << "Не удалось открыть файл\n";
        }
        fin.close();
    }

    void Save() override {
        ofstream fout(filename);
        fout << text;
        fout.close();
        cout << "Документ сохранён: " << filename << endl;
    }

    void SaveAs(string newName) override {
        filename = newName;
        Save();
    }

    void Print() override {
        cout << "В документе:\n";
        cout << text << endl;
    }

    void Close() override {
        cout << "Документ " << filename << " закрыт\n";
    }
    void Write(string line) {
        text += line + "\n";
    }
};

int main() {
    TextDocument txt;
    txt.Create();
    txt.Write("Hello, world!");
    txt.Print();
    txt.SaveAs("text.txt");
    txt.Close();
    return 0;
}

