$(function(){
  var slider = Swipe(document.getElementById('scroll_img'), {
  auto: 3000,
  continuous: true,
  callback: function(pos) {
    var i = bullets.length;
    while (i--) {
      bullets[i].className = ' ';
    }
    bullets[pos].className = 'on';
  }
});
var bullets = document.getElementById('scroll_position').getElementsByTagName('li');
$(function(){
  $('.scroll_position_bg').css({
    width:$('#scroll_position').width()
  });
});
})