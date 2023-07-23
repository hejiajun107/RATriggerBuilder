// See https://aka.ms/new-console-template for more information
using Sample;
using TriggerUtil;



var act1 = new YuriAct1();
//生成单独的ini
act1.Build("./Output/act1.ini");

//在已有的地图上附加触发
act1.Append("./Maps/test.mpr");

//生成预览图
act1.Preview("./Output/preview.html");

