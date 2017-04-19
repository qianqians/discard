#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

bool Import(const std::string& pFile){
	Assimp::Importer importer;

	const aiScene* scene = importer.ReadFile(pFile, 
		aiProcess_CalcTangentSpace | 
		aiProcess_Triangulate | 
		aiProcess_JoinIdenticalVertices | 
		aiProcess_SortByPType);

	if (!scene){
		return false;
	}

	return true;
}