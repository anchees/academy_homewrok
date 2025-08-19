#include <iostream>
#include <vector>
#include <string>

using namespace std;

class Component {
public:
    virtual void show(int indent = 0) = 0; 
};

class File : public Component {
    string name;
public:
    File(string n) : name(n) {}
    void show(int indent = 0) override {
        for (int i = 0; i < indent; i++) cout << " ";
        cout << "- " << name << endl;
    }
};

class Folder : public Component {
    string name;
    vector<Component*> children;
public:
    Folder(string n) : name(n) {}
    
    void add(Component* c) {
        children.push_back(c);
    }

    void show(int indent = 0) override {
        for (int i = 0; i < indent; i++) cout << " ";
        cout << "+ " << name << endl;
        for (auto c : children) {
            c->show(indent + 2);
        }
    }
};

int main() {
    Folder* root = new Folder("root");
    root->add(new File("file1.txt"));
    root->add(new File("file2.txt"));

    Folder* subFolder = new Folder("subfolder");
    subFolder->add(new File("file3.txt"));

    root->add(subFolder);
    
    cout<<"Выводим виртуальное дерево:\n";
    root->show();

    return 0;
}
