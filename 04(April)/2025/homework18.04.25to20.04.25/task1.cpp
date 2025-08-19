#include <iostream>
#include <string>
#include <vector>
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

class EditorApp {
private:
    vector<Document*> docs; 
public:
    void AddDocument(Document* d) {
        docs.push_back(d);
    }
    void CloseAll() {
        for (int i = 0; i < docs.size(); i++) {
            docs[i]->Close();
            delete docs[i];
        }
        docs.clear();
    }
};
