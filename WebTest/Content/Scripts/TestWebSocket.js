var ws1 = null;
var ws2 = null;
var openId = "";
var SeatNo = -1;
var pl = null;
$(function () {
    Init = function () {
        if (ws1 == null) {
            ws1 = new WebSocket("ws://47.101.130.0:8090");

            ws1.onopen = function (e) {

            }

            ws1.onerror = function (e) {
                alert("Error");
            }

            ws1.onmessage = function (e) {
                var text = e.data;
                var jsonObj = JSON.parse(text);
                var ac = jsonObj.Action;

                switch (ac) {

                    case 0:
                        pl = jsonObj.PlayerList;
                        var myOpenId = $("#OpenId").val();
                        $.each(pl, function (i) {
                            var stId = AddSeat(pl[i].SeatNo, pl[i].RemainCoins);

                            if (pl[i].CardList && pl[i].UserOpenId == myOpenId) {
                               
                                var card1 = GetCardName(pl[i].CardList[0]);
                                var card2 = GetCardName(pl[i].CardList[1]);
                                
                                var cardHtml = "<div>" + card1 + "</div><div>" + card2 + "</div>";
                              
                                var html = "<div>" + cardHtml + "</div>";

                                $("#" + stId).append(html);

                            }
                        });
                        tableCardList = jsonObj.TableCardList;
                        $.each(tableCardList, function (i) {
                            AddTableCard(tableCardList[i]);
                        });

                        break;
                    case 101:
                        SeatNo = jsonObj.SeatNo;
                        var found = false;
                        $.each(pl, function (i) {
                            if (pl[i].SeatNo == SeatNo) {
                                found = true;
                                return false;
                            }
                        });
                        if (!found)
                            AddSeat(SeatNo);
                        break;
                    case 1:
                        $("#Notice").text("等待玩家中。。。");
                        break;
                    case 2:
                    case 3:
                        $("#Notice").text("洗牌中。。。");
                        break;
                    case 4:
                        $("#Notice").text("洗牌完成，游戏中");
                        break;
                }

                $("#msg").text(text);

            }
        }

    }
    Init();
});

function GetCardName(CardObj) {
    var cardType = CardObj.CardType;
    var cardName = "";
    switch (cardType) {
        case 1:
            cardName = "[黑桃]"; break;
        case 2:
            cardName = "[红桃]"; break;
        case 3:
            cardName = "[梅花]"; break;
        case 4:
            cardName = "[方块]"; break;
    }
    cardName += "_" + CardObj.Value;
    return cardName;
}

function AddTableCard(CardObj)
{
    var card = GetCardName(CardObj);
    $(".TableArea").append('<div class="CardDiv">' + card + '</div>');
}

function AddSeat(SeatNo,RemainCoins) {
    var Id = "sn" + SeatNo;
    var consId = "coin"+SeatNo;
    var html = ' <div class="SeatDiv" id="' + Id + '">';
    html += '<div >No ' + SeatNo + '</div>';
    html += ' <img src="/Content/Images/seat.png" />';
    html += '<div id="' + consId + '" class="CoinDiv">' + RemainCoins + '</div>';
    html += '</div>';
   
    var SeatArea = $(".SeatArea");
    SeatArea.append(html);
    return Id;
}


function UserSitDown(n) {

    var openId = $("#OpenId").val();

    var json = 'UserSitDown {"OpenId":"' + openId + '",\
                                    "SeatNo":-1, \
                                    "Coins":100 \
}';
    ws1.send(json);

}

function EntryGame(n) {

    var openId = $("#OpenId").val();

    var json = 'EntryGame {"OpenId":"' + openId + '",\
                                    "UserName":"' + openId + '_Name", \
                                    "Weight":0 \
}';
    ws1.send(json);
}

function UserSitUp(n) {

    var openId = $("#OpenId").val();

    var json = 'UserSitUp {"OpenId":"' + openId + '",\
                                  }';
    ws1.send(json);

}

function UserExit(n) {

    var openId = $("#OpenId").val();

    var json = 'UserExit {"OpenId":"' + openId + '",}';
    ws1.send(json);

}

function Pass() {

}

function AllIn() {

}

function Follow() {

}

function AddCoins() {

}

function Test() {

    var openId = $("#OpenId").val();

    var json = 'BackHall {"OpenId":"' + openId + '",}';
    ws1.send(json);

}