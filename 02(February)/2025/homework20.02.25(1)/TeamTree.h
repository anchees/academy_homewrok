#ifndef TEAM_TREE_H
#define TEAM_TREE_H

#include <iostream>
#include "Team.h"
using namespace std;

struct TreeNode {
    Team data;
    TreeNode* left;
    TreeNode* right;

    TreeNode(Team t) : data(t), left(nullptr), right(nullptr) {}
};

class TeamTree {
private:
    TreeNode* root;

    void insert(TreeNode*& node, const Team& t) {
        if (!node) node = new TreeNode(t);
        else if (t.getPoints() > node->data.getPoints())
            insert(node->left, t);  
        else
            insert(node->right, t); 
    }

    void printTable(TreeNode* node) {
        if (!node) return;
        printTable(node->left);
        printTeam(node->data);
        printTable(node->right);
    }

    void printTeam(const Team& t) {
        cout << t.name << "\t| " << t.wins << " | " << t.losses << " | " << t.draws
             << " | " << t.goalsFor << " | " << t.goalsAgainst << " | "
             << t.getGoalDiff() << " | " << t.getPoints() << endl;
    }

public:
    TeamTree() : root(nullptr) {}

    void add(const Team& t) {
        insert(root, t);
    }

    void showTable() {
        cout << "Название команды | Победы | Поражения | Ничьи | Забитые | Пропущенные | Разница | Очки\n";
        cout << "-----------------|--------|-----------|--------|---------|-------------|---------|------\n";
        printTable(root);
    }
};

#endif 
