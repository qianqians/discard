#include "angmalloc.h"
#include <iostream>
#include <ctime>
#include <thread>
#include <atomic>
#include <vector>
#include <memory>

void test(){
	std::clock_t begin = clock();
	std::vector<int*> mem;
	for (int i = 10; i < 100000; i++) {
		for (int j = 0; j < 5; j++) {
			int* ret = (int*)angmalloc(i * sizeof(int));
			for (int k = 0; k < i; k++) {
				ret[k] = k;
			}
			mem.push_back(ret);
		}
		for (auto m : mem) {
			angfree(m);
		}
		mem.clear();
	}
	std::cout << "angmalloc" << std::clock() - begin << std::endl;
}

void test1() {
	std::clock_t begin = clock();
	int* ret = (int*)angmalloc(10 * sizeof(int));
	for (size_t i = 10; i < 100000; i++) {
		for (size_t j = 0; j < 5; j++) {
			ret = (int*)angrealloc(ret, (i + j) * sizeof(int));
			for (int k = 0; k < (i + j); k++) {
				ret[k] = k;
			}
		}
	}
	std::cout << "angmalloc" << std::clock() - begin << std::endl;
}

int main(){
	std::thread th1(test), th2(test), th3(test1), th4(test1);
	th1.join();
	th2.join();
	th3.join();
	th4.join();

	int in;
	std::cin >> in;
}