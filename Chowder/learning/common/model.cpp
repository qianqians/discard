#include "model.h"
#include "textures2D.h"

#include <iostream>

namespace common {
//
//Model::Model(std::string path) {
//	loadModel(path);
//}
//
//void Model::update_model(float dx, float dy) {
//	theta += -dx;
//	phi += -dy;
//
//	auto trans = filament::math::mat4f::rotation(theta, filament::math::float3(0.0, 1.0, 0.0));
//	_model = trans * filament::math::mat4f::rotation(phi, filament::math::float3(1.0, 0.0, 0.0));
//}
//
//void Model::Draw(common::program& _program) {
//	auto* pRTV = _program.swapchain->GetCurrentBackBufferRTV();
//	auto* pDSV = _program.swapchain->GetDepthBufferDSV();
//	immediate_context->SetRenderTargets(1, &pRTV, pDSV, Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
//	
//	// Clear the back buffer
//	const float ClearColor[] = { 0.350f, 0.350f, 0.350f, 1.0f };
//	immediate_context->ClearRenderTarget(pRTV, ClearColor, Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
//	immediate_context->ClearDepthStencil(pDSV, Diligent::CLEAR_DEPTH_FLAG, 1.f, 0, Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
//
//	for (auto& mesh : meshes) {
//		mesh->model(_model);
//		mesh->Draw(_program);
//	}
//}
//
//void Model::loadModel(std::string path) {
//	Assimp::Importer import;
//	const aiScene* scene = import.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs | aiProcess_CalcTangentSpace);
//
//	if (!scene || scene->mFlags & AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
//	{
//		std::cout << "ERROR::ASSIMP::" << import.GetErrorString() << std::endl;
//		return;
//	}
//	directory = path.substr(0, path.find_last_of('/'));
//
//	processNode(scene->mRootNode, scene);
//}

}