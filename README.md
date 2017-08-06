以下为本分支做的翻译:

如果您在README中找不到您要查找的信息。 请前往 [wiki](https://github.com/NiclasOlofsson/MiNET/wiki)

MiNET
=====

[![Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/xCNrhDd) 
[![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master?svg=true)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master) 
[![NuGet Version and Downloads count](https://buildstats.info/nuget/MiNET)](https://www.nuget.org/packages/MiNET) 

## 我可以在下载之前先测试一下吗？
是的你可以。 下面的一些是专业网络，有时有开发服务器运行，你可能会幸运地进入..(非本人的)<br>

IP: **CRISTALIX.PE**    
Port: **19132**  

IP: **play.infinite.pe**    
Port: **19132**    

IP: **pepirates.ru**    
Port: **19132** 

IP: **yodamine.com (dev server)**    
Port: **19132/19134**    

## 这是什么?

用C＃编写的基本的Minecraft Pocket Edition（MCPE）服务器。 目前的目标是为大型Minecraft多用户游戏提供商创建具有企业绩效的服务器。 当前的服务器每个实例处理10-100个用户，与MiNET的目标是处理数千个。 优先级也是创建界面，并在一定程度上建立最基本的企业游戏管理功能（用户，世界等）的现成实施。 随着所有可能性，MiNET将出现在Microsoft Cloud Ready产品中。

作为项目的一部分，我还提供最新的 [automatically generated MCPE Protocol Specification](/src/MiNET/MiNET/Net/MCPE%20Protocol%20Documentation.md). 这是通过使用XML和T4模板生成的通信代码的大部分的协同作用。

## 为什么做这个?
(下面的"我"并非本我)
"这实际上是一个宠物项目 - 一个真正的父子项目，我和我的儿子奥利弗6岁。 他正在推动对此的要求，对执行顺序进行优先排序。 他是在线MCPE游戏的粉丝，但仍然太年轻，无法发挥“真正的交易”。 他还缺乏耐力和对滞后性能不佳的服务器的理解，以及后果的后果。 他也不是杀死式游戏模式的大粉丝，所以这个子项目是让他创造出可以在MiNET中实现的新的kidz-friendly游戏模式。 奥利弗是最初负责创建我们项目范围的人。"

"而且他真的很喜欢现在告诉母亲妈妈，别打扰爸爸，现在正在为我工作！“ 6岁，但我觉得你得到的照片。 我知道他这样做。"

"最近kennyvv加入，成为了Oliver的团队的正式成员。 我仍然在考虑这个对父子项目概念的影响。 有可能被迫采用肯尼夫，我们会看到。 我猜，现在我将是一个老脾气暴躁的家伙，肯尼夫是明智的年轻人（聪明的孩子，是的）。"

跟随我在 <a href="https://twitter.com/NiclasOlofsson" class="twitter-follow-button" data-show-count="true" data-size="large" data-dnt="true">Twitter @NiclasOlofsson</a> 关于该项目的消息，或者简单地跟踪我倾向于评论的签名。

亦可以在kennyvv<a href="https://twitter.com/WuppDotNet" class="twitter-follow-button" data-show-count="true" data-size="large" data-dnt="true">Twitter @WuppDotNet</a> 上获取最新的消息。
 
MiNET通过AppVeyor的精彩服务运行CI。 目前主人的建立状态是...
[![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master)

## 可以做插件吗?

当然，API： [Plugin documention in the wiki](https://github.com/NiclasOlofsson/MiNET/wiki/Plugin-API-Documentation)

**请注意，插件系统不断更新，可能有些已经过时.**

## 我可以贡献吗?

当然可以，我们需要你同意以下规则:
1:您将使用与其余代码相同的编码风格。
2:除非您有权限，否则您不得从任何人或任何地方复制代码。
3:我们可以随时决定不要包含你的代码，我们可以对它进行更改。 所以在你向项目提出请求之前，请问更好。

此外，请确保加入我们以方便沟通。  
[![Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/xCNrhDd) 

## MiNET有没有官方论坛?

没有，但你可以问 [stack**overflow**](http://stackoverflow.com/questions/ask?tags=minet) 使用标签 *MiNET*. 

## 安装

### Window

查看视频 [MiNET quick windows tutorial](https://www.youtube.com/watch?v=AOgZx2vaIyw) by [Bamuel](https://github.com/Bamuel).

对于Windows安装，您可以选择从构建服务器下载二进制文件，或者从Visual Studio获取代码并进行编译。 请注意，MiNET需要.NET 4.6。

当从网络下载exe＆dll文件时，它们通常会被沙箱阻挡 - 您需要在属性窗口中右键单击并解除阻止，以使其无误运行。

### Mono (Linux & Mac)
MCPE 0.15引入了很多加密，这不是在Mono上实现的。 所以直到另一个解决方案出现，Mono的支持是不可能的。
~~[Mono Installation](https://github.com/NiclasOlofsson/MiNET/wiki/Running-MiNET-on-Linux)~~
