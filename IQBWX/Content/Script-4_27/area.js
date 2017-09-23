var Cities = {
    '北京市': ['市辖区', '所属县'],
    '天津市': ['市辖区', '所属县'],
    '河北省': ['石家庄市', '唐山市', '秦皇岛市', '邯郸市', '邢台市', '保定市', '张家口市', '承德市', '沧州市', '廊坊市', '衡水市'],
    '福建省': ['福州市', '厦门市', '莆田市', '三明市', '泉州市', '漳州市', '南平市', '龙岩市', '宁德地区'],
    '上海市': ['市辖区', '所属县'],
    '江苏省': ['南京市', '无锡市', '徐州市', '常州市', '苏州市', '南通市', '连云港市', '淮阴市', '盐城市', '扬州市', '镇江市', '泰州市', '宿迁市'],
    '内蒙古自治区': ['呼和浩特市', '包头市', '乌海市', '赤峰市', '呼伦贝尔盟', '兴安盟', '哲里木盟', '锡林郭勒盟', '乌兰察布盟', '伊克昭盟', '巴彦淖尔盟', '阿拉善盟'],
    '浙江省': ['杭州市', '宁波市', '温州市', '嘉兴市', '湖州市', '绍兴市', '金华市', '衢州市', '舟山市', '台州市', '丽水地区'],
    '山西省': ['太原市', '大同市', '阳泉市', '长治市', '晋城市', '朔州市', '忻州地区', '吕梁地区', '晋中地区', '临汾地区', '运城地区'],
    '辽宁省': ['沈阳市', '大连市', '鞍山市', '抚顺市', '本溪市', '丹东市', '锦州市', '营口市', '阜新市', '辽阳市', '盘锦市', '铁岭市', '朝阳市', '葫芦岛市'],
    '吉林省': ['长春市', '吉林市', '四平市', '辽源市', '通化市', '白山市', '松原市', '白城市', '延边朝鲜族自治州'],
    '黑龙江省': ['哈尔滨市', '齐齐哈尔市', '鸡西市', '鹤岗市', '双鸭山市', '大庆市', '伊春市', '佳木斯市', '七台河市', '牡丹江市', '黑河市', '绥化地区', '大兴安岭地区'],
    '安徽省': ['合肥市', '芜湖市', '蚌埠市', '淮南市', '马鞍山市', '淮北市', '铜陵市', '安庆市', '黄山市', '滁州市', '阜阳市', '宿州市', '六安地区', '宣城地区', '巢湖地区', '池州地区'],
    '广西壮族自治区': ['南宁市', '柳州市', '桂林市', '梧州市', '北海市', '防城港市', '钦州市', '贵港市', '玉林市', '南宁地区', '柳州地区', '贺州地区', '百色地区', '河池地区'],
    '河南省': ['郑州市', '开封市', '洛阳市', '平顶山市', '安阳市', '鹤壁市', '新乡市', '焦作市', '濮阳市', '许昌市', '漯河市', '三门峡市', '南阳市', '商丘市', '信阳市', '周口地区', '驻马店地区'],
    '湖北省': ['武汉市', '黄石市', '十堰市', '宜昌市', '襄樊市', '鄂州市', '荆门市', '孝感市', '荆州市', '黄冈市', '咸宁市', '恩施土家族苗族自治州', '省直辖县级行政单位'],
    '湖南省': ['长沙市', '株洲市', '湘潭市', '衡阳市', '邵阳市', '岳阳市', '常德市', '张家界市', '益阳市', '郴州市', '永州市', '怀化市', '娄底地区', '湘西土家族苗族自治州'],
    '江西省': ['南昌市', '景德镇市', '萍乡市', '九江市', '新余市', '鹰潭市', '赣州市', '瑞金市', '兴国县', '于都县', '宁都县', '宜春地区', '上饶地区', '吉安地区', '抚州地区'],
    '山东省': ['济南市', '青岛市', '淄博市', '枣庄市', '东营市', '烟台市', '潍坊市', '济宁市', '泰安市', '威海市', '日照市', '莱芜市', '临沂市', '德州市', '聊城市', '滨州地区', '菏泽地区'],
    '广东省': ['广州市', '韶关市', '深圳市', '珠海市', '汕头市', '佛山市', '江门市', '湛江市', '茂名市', '肇庆市', '惠州市', '梅州市', '汕尾市', '河源市', '阳江市', '清远市', '东莞市', '中山市', '潮州市', '揭阳市', '云浮市'],
    '陕西省': ['西安市', '铜川市', '宝鸡市', '咸阳市', '渭南市', '延安市', '汉中市', '安康地区', '商洛地区', '榆林地区'],
    '甘肃省': ['兰州市', '嘉峪关市', '金昌市', '白银市', '天水市', '酒泉地区', '张掖地区', '武威地区', '定西地区', '陇南地区', '平凉地区', '庆阳地区', '临夏回族自治州', '甘南藏族自治州'],
    '贵州省': ['贵阳市', '六盘水市', '遵义市', '铜仁地区', '南布依族苗族自治州', '毕节地区', '安顺地区', '黔东南苗族侗族自治州', '黔南布依族苗族自治州'],
    '青海省': ['西宁市', '海东地区', '海北藏族自治州', '黄南藏族自治州', '海南藏族自治州', '果洛藏族自治州', '玉树藏族自治州', '海西蒙古族藏族自治州'],
    '海南省': ['省所属市、县、岛', '海口市', '三亚市'],
    '重庆市': ['市辖区', '所属县'],
    '四川省': ['成都市', '自贡市', '攀枝花市', '泸州市', '德阳市', '绵阳市', '广元市', '遂宁市', '内江市', '乐山市', '南充市', '宜宾市', '广安市', '达川地区', '雅安地区', '阿坝藏族羌族自治州', '甘孜藏族自治州', '凉山彝族自治州', '巴中地区', '眉山地区', '资阳地区'],
    '云南省': ['昆明市', '曲靖市', '玉溪市', '昭通地区', '楚雄彝族自治州', '红河哈尼族彝族自治州', '文山壮族苗族自治州', '思茅地区', '西双版纳傣族自治州', '大理白族自治州', '保山地区', '德宏傣族景颇族自治州', '丽江地区', '怒江傈僳族自治州', '迪庆藏族自治州', '临沧地区'],
    '西藏自治区': ['拉萨市', '昌都地区', '山南地区', '日喀则地区', '那曲地区', '阿里地区', '林芝地区'],
    '宁夏回族自治区': ['银川市', '石嘴山市', '吴忠市', '固原地区'],
    '新疆维吾尔自治区': ['乌鲁木齐市', '克拉玛依市', '吐鲁番地区', '哈密地区', '昌吉回族自治州', '博尔塔拉蒙古自治州', '巴音郭楞蒙古自治州', '阿克苏地区', '克孜勒苏柯尔克孜自治州', '喀什地区', '和田地区', '伊犁哈萨克自治州', '伊犁地区', '塔城地区', '阿勒泰地区', '自治区直辖县级行政单位'],
    '台湾省': [''],
    '香港特别行政区': [''],
    '澳门特别行政区': ['']
};

/// <summary>
/// 设置省份
/// </summary>
function SetProvince(SelectId) {
    var Select_Province = document.getElementById(SelectId);
    if (Select_Province) {
        with (Select_Province) {
            if (citydata != null) {
                var leng = citydata.length;
                for (var i = 0; i < leng; i++) {
                    if (citydata[i]["V"] != null) {
                        options.add(new Option(citydata[i]["V"], citydata[i]["V"]));
                    }
                }
            }
        }
    }
}

/// <summary>
/// 设置省份对应的城市
/// </summary>
function SetCity(SelectId, Province) {
    var Select_City = document.getElementById(SelectId);
    if (Select_City) {
        with (Select_City) {
            var slen = options.length;
            for (var i = 0; i < slen; i++) {
                options[0] = null;
            }
            if (Cities[Province] == null)
                return;
            var len = Cities[Province].length;
            for (var i = 0; i < len; i++) {
                options[i] = new Option(Cities[Province][i], Cities[Province][i]);
            }
        }
    }
}

/// <summary>
/// 设置城市对应的地区
/// </summary>
function SetSection(ParentSelectId, SelectId, City) {
    var leng = citydata.length;
    var Select_obj = document.getElementById(SelectId);
    ClearOptions(SelectId, '', '');

    var province = $(ParentSelectId).value;

    for (var i = 0; i < leng; i++) {
        var obj = citydata[i]["C"];

        if (province == "北京市" || province == "天津市" || province == "上海市" || province == "重庆市") {
            if (province == citydata[i]["V"]) {
                var region = citydata[i]["C"];
                var len1 = region.length;
                for (var h = 0; h < len1; h++) {
                    Select_obj.options.add(new Option(region[h]["V"], region[h]["V"]));
                }
            }
        } else {
            var le = obj.length;
            for (var j = 0; j < le; j++) {
                if (obj[j]["V"] == City) {
                    if (obj[j]["C"] != null) {
                        var l = obj[j]["C"].length;
                        var obj2 = obj[j]["C"];
                        for (var k = 0; k < l; k++) {
                            Select_obj.options.add(new Option(obj2[k]["V"], obj2[k]["V"]));
                        }
                    }
                }
            }
        }
    }
}

/// <summary>
/// 给下拉列表增加一个选项
/// </summary>
function AddOption(SelectId, DefaultName, DefaultValue, Index, SelectedIndex) {
    var Select_obj = document.getElementById(SelectId);
    if (DefaultName != null && DefaultName.length > 0) {
        if (DefaultValue != null && DefaultValue.length > 0) {
            if (Index != null && Index >= 0) {
                Select_obj.options.add(new Option(DefaultName, DefaultValue), Index);
            } else {
                Select_obj.options.add(new Option(DefaultName, DefaultValue));
            }
        } else {
            if (Index != null && Index >= 0) {
                Select_obj.options.add(new Option(DefaultName, ''), Index);
            } else {
                Select_obj.options.add(new Option(DefaultName, ''));
            }
        }
    }
    if (SelectedIndex != null && SelectedIndex >= 0) {
        Select_obj.selectedIndex = 0;
    }
}

/// <summary>
/// 清空下拉列表
/// </summary>
function ClearOptions(SelectId, DefaultName, DefaultValue) {
    var Select_obj = document.getElementById(SelectId);
    var slen = Select_obj.length;
    for (var i = 0; i < slen; i++) {
        Select_obj.options[0] = null;
    }
    if (DefaultName != null && DefaultName.length > 0) {
        if (DefaultValue != null && DefaultValue.length > 0) {
            Select_obj.options.add(new Option(DefaultName, DefaultValue));
        } else {
            Select_obj.options.add(new Option(DefaultName, ''));
        }
    }
}


/// <summary>
/// 选中指定值
/// </summary>
function SelectByValue(selectId, Value) {
    var select_obj = document.getElementById(selectId);
    if (select_obj != null) {
        with (select_obj) {
            var len = length;
            for (var i = 0; i < len; i++) {
                if (options[i].value == Value) {
                    var va = select_obj.selectvalue;
                    selectedIndex = i;
                    if (onchange != null || onchange != undefined)
                        onchange();
                }
            }
        }
    }
}