#include <Hemsleya/base/concurrent/parallel/parallel_sort.h>

int main(){
	std::vector<int> v;
	for (int i = 0; i < 2000; i++){
		v.push_back(rand());
	}
	
	Hemsleya::parallel::parallel_sort<int>(v);

	int out = 0;
	for (size_t i = 0; i < v.size(); i++){
		if (out > v[i]){
			abort();
		}
		
		out = v[i];
	}

	return 0;
}