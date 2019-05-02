var ws1 = null;
var ws2 = null;
var openId = "";
var MySeatNo = -1;
var pl = null;

var processClock = null;
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
                    //进入游戏
                    case 0:
                        var GameInfo = jsonObj.GameInfo;

                        SetGameInfo(GameInfo);

                        var gameStatus = GameInfo.GameStatus;
                        $("#Notice").text(GetStatusName(gameStatus));

                        pl = jsonObj.PlayerList;
                        var myOpenId = $("#OpenId").val();
                        $.each(pl, function (i) {

                            var stId = AddSeat(pl[i].SeatNo, pl[i].RemainCoins, pl[i].UserOpenId);

                            if (pl[i].CardList.length != 0 && pl[i].UserOpenId == myOpenId) {

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
                        coins = jsonObj.RemainCoins;
                        var myOpenId = $("#OpenId").val();

                        AddSeat(SeatNo, coins, myOpenId).text();
                        break;
                    case 1:
                        $("#Notice").text(GetStatusName(1));

                        break;
                    case 2:
                        $("#Notice").text(GetStatusName(2));
                        break;
                    case 3:
                        $("#Notice").text(GetStatusName(3));
                        break;
                    case 4:

                        var gi = jsonObj.GameInfo;
                        SetGameInfo(gi);

                        var betUserId = jsonObj.GameInfo.CurBetUserOpenId;
                        //    var myOpenId = $("#OpenId").val();

                        $(".SeatArea").children().each(function () {
                            var seatOpenId = $(this).find("#seatUserId").text();
                            if (seatOpenId == betUserId) {
                                var seatNo = $(this).attr("Id");
                                StartWaitUser(seatNo);
                                return false;
                            }

                        });


                        $("#Notice").text(GetStatusName(4));
                        break;
                    case 50:
                        $("#Notice").text(GetStatusName(50));
                }

                $("#msg").text(text);

            }
        }

    }
    Init();

    // TestInit();
});

function TestInit() {
    var html = '<div class="progress">';
    html += '<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 40%;">';
    html += '<span class="sr-only">40% 完成</span>';
    html += '</div></div>';
    $("#UserArea").append(html);
}

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

function AddTableCard(CardObj) {
    var card = GetCardName(CardObj);
    $(".TableArea").append('<div class="CardDiv">' + card + '</div>');
}

function AddSeat(SeatNo, RemainCoins, userId) {
    var Id = "sn" + SeatNo;
    var consId = "coin" + SeatNo;
    var myOpenId = $("#OpenId").val();

    if (userId == myOpenId) {
        MySeatNo = SeatNo;
    }
    var html = ' <div class="SeatDiv" id="' + Id + '">';
    html += "<div id='seatUserId'>" + userId + "</div>";
    html += '<div class="progress" style="width:120px;">';
    html += '<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" pn=0>';
    //html += '<span class="sr-only"></span>';
    html += '</div></div>';
    html += '<div id="SeatNo">No ' + SeatNo + '</div>';

    html += ' <img src="/Content/Images/seat.png" />';
    html += '<div id="' + consId + '" class="CoinDiv">' + RemainCoins + '</div>';
    html += '</div>';

    var SeatArea = $(".SeatArea");
    SeatArea.append(html);
    // StartWaitUser(Id);
    return Id;
}


function SetGameInfo(gi) {
    if (gi != "" || gi != null) {
        $("#giStatus").text("游戏状态：" + GetStatusName(gi.GameStatus));
        $("#giTurn").text("局：" + gi.GameTurn);
        $("#giCurOpenId").text("当期出牌人：" + gi.CurBetUserOpenId);
        $("#giSmall").text("小盲注：" + gi.SmallBetUserOpenId);
        $("#giBig").text("大盲注：" + gi.BigBetUserOpenId);
        $("#giDot").text("Dot：" + gi.DotUserOpenId);

        $("#giReqCoins").text("需押注：" + gi.CurRequreCoins);
    }
}
function StartWaitUser(seatId) {

    processClock = setInterval("WaitingUser(seatId='" + seatId + "')", 1000);

}

function WaitingUser(seatId) {

    var processBar = $("#" + seatId).find(".progress-bar");
    var pn = parseInt(processBar.attr("pn"));

    pn += 6.6;
    processBar.attr("pn", pn);
    processBar.css("width", pn + "%");
    if (pn > 100)
        clearInterval(processClock)
}



function GetStatusName(status) {
    switch (status) {
        case 0: return "没有游戏";
        case 1: return "等待玩家中。。。";
        case 2:
        case 3:
            return "洗牌中。。。";
        case 4:
            return "洗牌完成，游戏中";
        case 10:
            return "游戏中";
        case 50:
            return "游戏结束，翻牌中";
        case 51:
            return "游戏结束，结算";

    }
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
    var openId = $("#OpenId").val();

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

function StartStuffleGame() {

    var openId = $("#OpenId").val();

    $(".SeatArea").find(".progress-bar").each(function () {

        var obj = $(this);
        obj.attr("pn", 0);
        obj.css("width", "0%");




    });



    var json = 'TestStartGame {"OpenId":"' + openId + '",\
                                    "RoomCode":"04240608",\
}';
    ws1.send(json);
}