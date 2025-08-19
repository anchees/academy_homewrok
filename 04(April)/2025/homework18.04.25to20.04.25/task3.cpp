#include <iostream>
#include <fstream>
#include <string>
using namespace std;

class Document {
public:
    virtual void Create() = 0;
    virtual void Open(string fname) = 0;
    virtual void Save() = 0;
    virtual void SaveAs(string newName) = 0;
    virtual void Print() = 0;
    virtual void Close() = 0;
    virtual ~Document() {}
};

class ImageDocument : public Document {
private:
    string filename;
public:
    void Create() override {
        filename = "new.png";
        ofstream fout(filename); 
        fout.close();
    }

    void Open(string fname) override {
        filename = fname;
    }

    void Save() override {
        ofstream fout(filename); 
        fout.close();
    }

    void SaveAs(string newName) override {
        filename = newName;
        ofstream fout(filename);
        fout.close();
    }

    void Print() override {
        cout << "Печать изображения: " << filename << endl;
    }

    void Close() override {
        cout << "Файл закрыт: " << filename << endl;
    }
};

int main() {
    ImageDocument img;
    img.Create();         
    img.SaveAs("test.png"); 
    img.Print();          
    img.Close();          
    return 0;
}

