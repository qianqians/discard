#include "program.h"

namespace common {
//
//program::program(Diligent::RefCntAutoPtr<Diligent::ISwapChain> _swapchain, 
//                 Diligent::RefCntAutoPtr<Diligent::IEngineFactory> _engine_factory, 
//                 Diligent::RefCntAutoPtr<Diligent::IRenderDevice> _device,
//                 const char* _vsName, const char* _fsName) {
//
//    swapchain = _swapchain;
//    engine_factory = _engine_factory;
//    device = _device;
//
//    // Pipeline state object encompasses configuration of all GPU stages
//
//    Diligent::GraphicsPipelineStateCreateInfo PSOCreateInfo;
//
//    // Pipeline state name is used by the engine to report issues.
//    // It is always a good idea to give objects descriptive names.
//    PSOCreateInfo.PSODesc.Name = "Cube PSO";
//
//    // This is a graphics pipeline
//    PSOCreateInfo.PSODesc.PipelineType = Diligent::PIPELINE_TYPE_GRAPHICS;
//
//    // clang-format off
//    // This tutorial will render to a single render target
//    PSOCreateInfo.GraphicsPipeline.NumRenderTargets = 1;
//    // Set render target format which is the format of the swap chain's color buffer
//    PSOCreateInfo.GraphicsPipeline.RTVFormats[0] = _swapchain->GetDesc().ColorBufferFormat;
//    // Set depth buffer format which is the format of the swap chain's back buffer
//    PSOCreateInfo.GraphicsPipeline.DSVFormat = _swapchain->GetDesc().DepthBufferFormat;
//    // Primitive topology defines what kind of primitives will be rendered by this pipeline state
//    PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = Diligent::PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
//    // Cull back faces
//    PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = Diligent::CULL_MODE_BACK;
//    // Enable depth testing
//    PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = true;
//    // clang-format on
//
//    Diligent::ShaderCreateInfo ShaderCI;
//    // Tell the system that the shader source code is in HLSL.
//    // For OpenGL, the engine will convert this into GLSL under the hood.
//    ShaderCI.SourceLanguage = Diligent::SHADER_SOURCE_LANGUAGE_HLSL;
//
//    // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
//    ShaderCI.UseCombinedTextureSamplers = true;
//
//    // In this tutorial, we will load shaders from file. To be able to do that,
//    // we need to create a shader source stream factory
//    Diligent::RefCntAutoPtr<Diligent::IShaderSourceInputStreamFactory> pShaderSourceFactory;
//    _engine_factory->CreateDefaultShaderSourceStreamFactory(nullptr, &pShaderSourceFactory);
//    ShaderCI.pShaderSourceStreamFactory = pShaderSourceFactory;
//    // Create a vertex shader
//    Diligent::RefCntAutoPtr<Diligent::IShader> pVS;
//    {
//        ShaderCI.Desc.ShaderType = Diligent::SHADER_TYPE_VERTEX;
//        ShaderCI.EntryPoint = "main";
//        ShaderCI.Desc.Name = "Cube VS";
//        ShaderCI.FilePath = _vsName;
//        _device->CreateShader(ShaderCI, &pVS);
//
//        Diligent::BufferDesc BuffDesc;
//        BuffDesc.Name = "Constant buffer";
//        BuffDesc.Usage = Diligent::USAGE_DYNAMIC;
//        BuffDesc.BindFlags = Diligent::BIND_UNIFORM_BUFFER;
//        BuffDesc.CPUAccessFlags = Diligent::CPU_ACCESS_WRITE;
//        BuffDesc.Size = sizeof(Constants);
//        _device->CreateBuffer(BuffDesc, nullptr, &pConstants);
//        VERIFY_EXPR(pConstants != nullptr);
//    }
//
//    // Create a pixel shader
//    Diligent::RefCntAutoPtr<Diligent::IShader> pPS;
//    {
//        ShaderCI.Desc.ShaderType = Diligent::SHADER_TYPE_PIXEL;
//        ShaderCI.EntryPoint = "main";
//        ShaderCI.Desc.Name = "Cube PS";
//        ShaderCI.FilePath = _fsName;
//        _device->CreateShader(ShaderCI, &pPS);
//    }
//
//    // clang-format off
//    // Define vertex shader input layout
//    Diligent::LayoutElement LayoutElems[] =
//    {
//        // Attribute 0 - vertex position
//        Diligent::LayoutElement(0, 0, 3, Diligent::VT_FLOAT32, false),
//        Diligent::LayoutElement(1, 0, 3, Diligent::VT_FLOAT32, false),
//        Diligent::LayoutElement(2, 0, 3, Diligent::VT_FLOAT32, false),
//        // Attribute 1 - vertex color
//        Diligent::LayoutElement(3, 0, 2, Diligent::VT_FLOAT32, false)
//    };
//    // clang-format on
//    PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements = LayoutElems;
//    PSOCreateInfo.GraphicsPipeline.InputLayout.NumElements = _countof(LayoutElems);
//
//    PSOCreateInfo.pVS = pVS;
//    PSOCreateInfo.pPS = pPS;
//
//    // Define variable type that will be used by default
//    PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = Diligent::SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;
//
//    // clang-format off
//    // Shader variables should typically be mutable, which means they are expected
//    // to change on a per-instance basis
//    Diligent::ShaderResourceVariableDesc Vars[] =
//    {
//        {Diligent::SHADER_TYPE_PIXEL, "s_diffuse", Diligent::SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC},
//        {Diligent::SHADER_TYPE_PIXEL, "s_specular", Diligent::SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC},
//        {Diligent::SHADER_TYPE_PIXEL, "s_normal", Diligent::SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC}
//    };
//    // clang-format on
//    PSOCreateInfo.PSODesc.ResourceLayout.Variables = Vars;
//    PSOCreateInfo.PSODesc.ResourceLayout.NumVariables = _countof(Vars);
//
//    // clang-format off
//    // Define immutable sampler for g_Texture. Immutable samplers should be used whenever possible
//    Diligent::SamplerDesc SamLinearClampDesc
//    {
//        Diligent::FILTER_TYPE_LINEAR, Diligent::FILTER_TYPE_LINEAR, Diligent::FILTER_TYPE_LINEAR,
//        Diligent::TEXTURE_ADDRESS_CLAMP, Diligent::TEXTURE_ADDRESS_CLAMP, Diligent::TEXTURE_ADDRESS_CLAMP
//    };
//    Diligent::ImmutableSamplerDesc ImtblSamplers[] =
//    {
//        {Diligent::SHADER_TYPE_PIXEL, "s_diffuse", SamLinearClampDesc},
//        {Diligent::SHADER_TYPE_PIXEL, "s_specular", SamLinearClampDesc},
//        {Diligent::SHADER_TYPE_PIXEL, "s_normal", SamLinearClampDesc}
//    };
//    // clang-format on
//    PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImtblSamplers;
//    PSOCreateInfo.PSODesc.ResourceLayout.NumImmutableSamplers = _countof(ImtblSamplers);
//
//    _device->CreateGraphicsPipelineState(PSOCreateInfo, &pso);
//
//    //// Since we did not explcitly specify the type for 'Constants' variable, default
//    //// type (SHADER_RESOURCE_VARIABLE_TYPE_STATIC) will be used. Static variables never
//    //// change and are bound directly through the pipeline state object.
//    //m_pPSO->GetStaticVariableByName(SHADER_TYPE_VERTEX, "Constants")->Set(m_VSConstants);
//
//    // Create a shader resource binding object and bind all static resources in it
//    pso->CreateShaderResourceBinding(&srb, true);
//
//    srb->GetVariableByName(Diligent::SHADER_TYPE_VERTEX, "cbConstants")->Set(pConstants);
//    srb->GetVariableByName(Diligent::SHADER_TYPE_PIXEL, "cbConstants")->Set(pConstants);
//}

}