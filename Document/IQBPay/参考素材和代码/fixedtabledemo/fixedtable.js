/* Created By jcheng.matt
 * Date: 2016-10-08
 */

(function ($) {

    // 固定表头表格对象
    var fixedtable = function (setting, obj) {
        this.setting = setting;
        this.obj = obj;
    };

    fixedtable.prototype.redraw = function () {
        var obj = this;
        // 表头
        var tbHeader = this.getHeader();
        // 表内容
        var tbBody = '<div class="divB">' + this.getBody() + '</div>';
        // 合并
        var html = '<div class="fix-header-table">'
        html += tbHeader;
        html += tbBody;
        html += '</div>';

        $(this.obj).empty().append(html);
        $(this.obj).find('.sortable').click(function (event) { obj.sort(event); });
        $(this.obj).find('tr[data-row-index]').click(function (event) { obj.select(event); });

        // 绑定resize事件
        $(window).resize(function () { obj.resize(); });

        this.resize();
    }

    fixedtable.prototype.getHeader = function () {
        var header = this.setting.header;
        var html = '<div class="divH"><table>';
        html += this.getTrForTdWidth();
        html += '<tr class="table-header">';
        for (var i = 0; i < header.length; i++) {
            var headerClass = header[i].headerClass ? header[i].headerClass : '';
            if (header[i].sortable) {
                html += '<td data-index="' + i + '" class="sortable ' + headerClass + '" ><div>' + header[i].header + '</div></td>';
            }
            else {
                html += '<td data-index="' + i + '" class="' + headerClass + '"><div>' + header[i].header + '</div></td>';
            }
        }
        html += '</tr>';
        html += '</table></div>';
        return html;
    }

    fixedtable.prototype.getBody = function () {
        var header = this.setting.header;
        var list = this.setting.data;
        var html = '<table>';
        html += this.getTrForTdWidth();
        for (var i = 0; i < list.length; i++) {
            html += '<tr class="detail" data-row-index="' + i + '">';
            for (var j = 0; j < header.length; j++) {
                var prop = header[j].dataIndex;
                var formatter = header[j].formatter ? header[j].formatter : this.setting.formatter;
                var align = header[j].textAlign ? header[j].textAlign : this.setting.textAlign;
                html += '<td style="text-align:' + align + '">' + formatter(list[i][prop], i, list[i]) + '</td>'
            }
            html += '</tr>';
        }
        html += '</table>';
        return html;
    }

    fixedtable.prototype.getTrForTdWidth = function () {
        var header = this.setting.header;
        var html = '<tr class="trForTdWidth">';
        for (var i = 0; i < header.length; i++) {
            html += '<td style="width:' + header[i].width + ';"></td>';
        }
        html += '</tr>';
        return html;
    }

    fixedtable.prototype.sort = function (event) {
        var obj = this;
        var header = this.setting.header;
        var index = +$(event.target).attr('data-index');
        var isDescending;
        if (!$(event.target).hasClass('asc')) {
            isDescending = false;
            $(event.target).addClass('asc');
        }
        else {
            isDescending = true;
            $(event.target).removeClass('asc');
        }
        list.sort(function (a, b) {
            var dataIndex = header[index].dataIndex;
            var compare = header[index].compare ? header[index].compare : this.setting.compare;
            return (isDescending ? 1 : -1) * compare(a[dataIndex], b[dataIndex]);
        });
        $(this.obj).find('.divB').html(this.getBody());
        $(this.obj).find('tr[data-row-index]').click(function (event) { obj.select(event); });
        $(this.obj).find('tr[data-row-index]').first().click();
    }

    fixedtable.prototype.select = function (event) {
        if ($(event.currentTarget).hasClass('selected')) {
            return;
        }

        $(event.currentTarget).siblings().removeClass('selected');
        $(event.currentTarget).addClass('selected');

        if (this.setting.rowClick) {
            this.setting.rowClick(event)
        }
    }

    fixedtable.prototype.resize = function () {
        var fixedTable = $(this.obj);

        // 设置表体高度
        fixedTable.find('.divB').height(fixedTable.height() - fixedTable.find('.divH').height());

        // ie7下，width 100%包含父元素的滚动条的宽度，需特殊处理
        if (/MSIE\s*7/.test(navigator.userAgent)) {
            var hasVerticalScroll = fixedTable.find('.divB').height() < fixedTable.find('.divB table').height();
            if (hasVerticalScroll) {
                fixedTable.find('.divB table').width(fixedTable.find('.divB').width() - 17);
            }
        }

        // 设置表头与表体等宽（出现纵向滚动条会导致不等宽）
        fixedTable.find('.divH, .divB').css('min-width', '');
        fixedTable.find('.divH table').width(fixedTable.find('.divB table').width());

        var hasScroll = fixedTable.find('.divB table').width() > fixedTable.width();
        if (hasScroll) {
            // 当出现横向滚动条时，表体高度需减去滚动条的高度
            fixedTable.find('.divB').height(fixedTable.height() - fixedTable.find('.divH').height() - 17);
            // min-width的宽度包含滚动条的宽度，当出现纵向滚动条时需
            fixedTable.find('table').each(function () {
                var hasVerticalScroll = fixedTable.find('.divB').height() < fixedTable.find('.divB table').height();
                $(this).parent().css('min-width', ($(this).width() + (hasVerticalScroll ? 17 : 0)) + 'px');
            });
        }
    }

    $.fn.fixedtable = function (options) {
        var fixedTableObj = this.prop('obj');
        if (!fixedTableObj) {
            var setting = $.extend({}, $.fn.fixedtable.defaults);
            fixedTableObj = new fixedtable(setting, this[0]);
            this.prop('obj', fixedtable);
        }
        if (options) {
            fixedTableObj.setting = $.extend({}, $.fn.fixedtable.defaults, options);
            fixedTableObj.redraw();
        }


        return {
            resize: function () {
                fixedTableObj.resize();
            }
        };
    };

    $.fn.fixedtable.defaults = {
        header: [],
        data: [],
        textAlign: 'center',
        formatter: function (arg) {
            return arg;
        },
        compare: function (arg1, arg2) {
            if (arg1 < arg2) {
                return -1;
            }
            else if (arg1 > arg2) {
                return 1;
            }
            else {
                return 0;
            }
        },
        rowClick: null
    }
} (jQuery));