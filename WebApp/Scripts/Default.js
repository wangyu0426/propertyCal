//NOTE: links and params are Case-Senstive!!!
var HLJLink = 'http://www.hlj.com';
var gundamCategoryLink = '/top/Gun';
var actionfigureCategoryLink = '/top/Act';
var waitMilsec = 10000; //wait 10 sec don't break HLJ's server!! total about 1.5 hours to load 400 items
var HLJ = {
    //stock: In Stock, Low Stock, Order Stop, Backordered, Preorder, XXX Released, SALE!, Product Spotlight,
    //parent: http://...,
    //link: http://....,
    //thumb: http://.....,
    //name: some name,
    //pithy: from brand... 

    //images: [http://...],
    //priceMsrp: ¥2,381,
    //priceSale: ¥2,262,
    //priceForex: USD $18.01; €16.20,
    //code: BAN1234,
    //description: blah blah...,
    //series: Gundam,
    //seriesLink: http://.....,
    //released: May 2013,
    //dimension: 39.4 x 31.4 x 12.6 cm / 1170g,
    //skillLv: 3,
    //cement: no, 
    //paint: yes, 
};
//Gundam possible Grades: FW, SD, RG, HG, MG / No Scale, 1/144, 1/100, 1/60 

$(document).ready(function () {
    // /Fig  /Act /Gun /Air/Dol /Mil  /Sci/Nav    /Sup
    //loadCategoryList(HLJ + actionfigureCategoryLink);
    //loadCategoryList(HLJ + gundamCategoryLink);
    //loadSearchList('http://www.hlj.com/scripts/hljlist/?SeriesID=2881');
    //loadSearchList('http://www.hlj.com/scripts/hljlist?SeriesID=5123&amp;Maker1=DIG&amp;Sort=releaseDate+desc');

    $('.btn-gundam').click(function () {
        resetProductList();
        loadCategoryList(HLJ, HLJLink + '/top/Gun');
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=649'); //Gundam
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=651'); //Gundam 0079
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=671'); //Gundam Seed
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=675'); //Mobile Suit Gundam The Origin
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=1458'); //SD Gundam
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=1926'); //Zeta Gundam
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=2881'); //Action Base
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=3524'); //Gundam UC Unicorn
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=5727'); //Gundam Build Fighters
        loadSearchList(HLJ, HLJLink + '/scripts/hljlist?SeriesID=6067'); //Gundam Reconguista in G
        
    });
    $('.btn-anime-figures').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Fig');});
    $('.btn-action-figures').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Act');});
    $('.btn-aircraft').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Air');});
    $('.btn-dolls').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Dol');});
    $('.btn-military').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Mil');});
    $('.btn-scifi').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Sci');});
    $('.btn-nav').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Nav');});
    $('.btn-supplies').click(function () {resetProductList();loadCategoryList(HLJ, HLJLink + '/top/Sup');});
    $('.btn-reset-list').click(function() { resetProductList(); });
    $('.btn-load-details').click(function () { loadProductDetails(HLJ, function() { showProductList(HLJ) }); });
});
function resetProductList() {
    HLJ = {};
    $('.product-list').html('');
}
function loadProductDetails(listItems, cb, i) {
    if (i === undefined) {
        i = 1;
        // Shallow copy
        listItems = jQuery.extend({}, listItems);
        if (listItems.length > 0) $('.product-details-sum').html('loading...');
        // Deep copy
        //var listItems = jQuery.extend(true, {}, listItems);
    }
    var item = pop(listItems);
    if (item) {
        setTimeout(function() {
            getData('api/html/?url=' + encodeURIComponent(item.link), function (data) {
                item.images = allElsAttr($(data.html), '.prod-gallery img', 'src');
                item.stock = firstEl($(data.html), '.prod-status-header').text().trim();
                item.priceMsrp = firstEl($(data.html), '.msrp.strike').text().trim();
                item.priceSale = firstEl($(data.html), '.sale-price').text().trim();
                item.priceForex = firstEl($(data.html), '.sale-currency').text().trim();
                item.series = firstEl($(data.html), 'dd.series').text().trim();
                item.seriesLink = HLJLink + firstEl($(data.html), 'dd.series a').attr('href');
                item.released = firstEl($(data.html), 'dd.releaseDate').text().trim();
                item.dimension = firstEl($(data.html), 'dd.itemDim').text().trim();
                item.code = firstEl($(data.html), 'span[itemprop="sku"]').text().trim();
                item.description = firstEl($(data.html), 'span[itemprop="description"]').text().trim();
                if (firstEl($(data.html), '.assemblyGuide .skill1')) item.skillLv = '1';
                if (firstEl($(data.html), '.assemblyGuide .skill2')) item.skillLv = '2';
                if (firstEl($(data.html), '.assemblyGuide .skill3')) item.skillLv = '3';
                if (firstEl($(data.html), '.assemblyGuide .skill4')) item.skillLv = '4';
                if (firstEl($(data.html), '.assemblyGuide .skill5')) item.skillLv = '5';
                if (firstEl($(data.html), '.assemblyGuide .skill5')) item.skillLv = '5';
                if (firstEl($(data.html), '.assemblyGuide .cement_n')) item.cement = 'no';
                if (firstEl($(data.html), '.assemblyGuide .cement_y')) item.cement = 'yes';
                if (firstEl($(data.html), '.assemblyGuide .paint_n')) item.paint = 'no';
                if (firstEl($(data.html), '.assemblyGuide .paint_y')) item.paint = 'yes';
                $('.product-details-sum').html('Got details: ' + i);
                //if (Object.keys(listItems).length <= 0) cb();
                //else loadProductDetails(listItems, cb, ++i);
                cb();
                loadProductDetails(listItems, cb, ++i);
            });
        }, waitMilsec);
    }
}
//just to load it up on page for debugging, making use I load the correct information
function showProductList(listItems) {
    var html = '<table cellpadding="10" align="center" border="1">';
    html += '<tr>' +
        '<th>thumb</th>' +
        '<th>name</th>' +
        '<th>pithy</th>' +
        '<th>code</th>' +
        '<th>series</th>' +
        '<th>priceMsrp</th>' +
        '<th>priceSale</th>' +
        '<th>priceForex</th>' +
        '<th>released</th>' +
        '<th>stock</th>' +
        '<th>images</th>' +
        '<th>dimension</th>' +
        '<th>skillLv</th>' +
        '<th>cement</th>' +
        '<th>paint</th>' +
        '<th>description</th>' +
        '<th>parent</th>' +
        '<th>seriesLink</th>' +
        '<th>link</th>' +
        '</tr>';
    var keys = Object.keys(listItems);
    Enumerable.from(keys).forEach(function (key) {
        var item = listItems[key];
        html += '</tr>';
        html += '<td class="btn-product-detail"><img src="' + item.thumb + '"  /></td>';
        html += '<td>' + item.name + '</td>';
        html += '<td>' + item.pithy + '</td>';
        html += '<td>' + item.code + '</td>';
        html += '<td>' + item.series + '</td>';
        html += '<td><strike>' + item.priceMsrp + '</strike></td>';
        html += '<td>' + item.priceSale + '</td>';
        html += '<td>' + item.priceForex + '</td>';
        html += '<td>' + item.released + '</td>';
        html += '<td style=" text-align: center; color: white; ' + getStockColor(item.stock) + '"><b>' + item.stock + '</b></td>';
        //html += '<td><div style="width: 800px;">' + (item.images ? Enumerable.from(item.images).select(function (img) { return '<img style="float:left;" src="' + img + '" />'; }).toArray().join('') : '') + '</div></td>';
        html += '<td><div style="width: 800px;">' + (item.images ? Enumerable.from(item.images).select(function (img) { return '<img style="float:left;" src="' + img.replace(/thumb_/g, '') + '" />'; }).toArray().join('') : '') + '</div></td>';
        html += '<td>' + item.dimension + '</td>';
        html += '<td>' + item.skillLv + '</td>';
        html += '<td>' + item.cement + '</td>';
        html += '<td>' + item.paint + '</td>';
        html += '<td><div style="width: 600px;">' + item.description + '</div></td>';
        html += '<td>' + item.parent + '</td>';
        html += '<td>' + item.seriesLink + '</td>';
        html += '<td class="link">' + item.link + '</td>';
        html += '</tr>';
    });
    html += '</table>';
    $('.product-list').html(html);
    $('.product-list-sum').html('<p><b>' + keys.length + ' item(s)</b></p>');
    $('.btn-product-detail').click(function(td) {
        var link = firstEl($($(td.currentTarget).closest('tr')), '.link').text();
        var item = listItems[link];
        loadProductDetails({ link: item }, function () { showProductList(listItems); });
    });
}
//load category list, auto find pages async, Summary info WITHOUT pricing. But we load price with item details later.
//Note: 'Product Spotlight' item actually a search result list, when we see that we reroute to let loadSearchList do the job
function loadCategoryList(listItems, pagelink, pageNo) {
    if (!pageNo) pageNo = 1; 
    getData('api/html/?url=' + encodeURIComponent(pagelink + '?page=' + pageNo), function (data) {
        Enumerable.from(allEl($(data.html), '.topItemBlock_float')).forEach(function (el) {
            var catItem = {
                parent: pagelink + pageNo,
                name: firstEl(el, '.title').html().trim(),
                code: firstEl(el, 'a').attr('href').replace(/\/product\//, '').toUpperCase().trim(),
                link: HLJLink + firstEl(el, 'a').attr('href').trim(),
                thumb: firstEl(el, 'a img').css("background-image").replace(/.*\s?url\([\'\"]?/, '').replace(/[\'\"]?\).*/, '').trim(),
                stock: firstEl(el, '.orderstatus_text') ? firstEl(el, '.orderstatus_text').html().trim() : "SALE!",
                pithy: firstEl(el, '.pithy').html().trim()
            };
            if (catItem.stock === 'Product Spotlight') loadSearchList(listItems, catItem.link);
            else listItems[catItem.link] = catItem;
        });
        if (!isListLastPage(data)) loadCategoryList(listItems, pagelink, ++pageNo);
        else {
            showProductList(listItems);
        }
    });
}
//load search list, auto find pages async. Summary with pricing, but we overwrite it later with item details later anyway.
function loadSearchList(listItems, pagelink, pageNo) {
    if (!pageNo) pageNo = 1;
    getData('api/html/?url=' + encodeURIComponent(pagelink + '&page=' + pageNo), function (data) {
        Enumerable.from(allEl($(data.html), '.tbl-searchresults tr')).forEach(function (el) {
            var srhItem = {
                parent: pagelink + pageNo,
                name: firstEl(el, 'td.centered a').attr('title').trim(),
                code: firstEl(el, 'td.centered a img').attr('alt').trim(),
                link: HLJLink + firstEl(el, 'td.centered a').attr('href').trim(),
                thumb: firstEl(el, 'td.centered a img').css("background-image").replace(/.*\s?url\([\'\"]?/, '').replace(/[\'\"]?\).*/, '').trim(),
                stock: firstEl(el, '.f-iteminfo span.p4tb').html().trim(),
                pithy: firstEl(el, '.f-iteminfo span.med-text').html().trim()
            };
            listItems[srhItem.link] = srhItem;
        });
        if (!isListLastPage(data)) loadSearchList(listItems, pagelink, ++pageNo);
        else {
            showProductList(listItems);
        }
    });
}
//detect if it is last page of a category list or a search retush list, both can use 
function isListLastPage(data) {
    var pager = firstEl($(data.html), '.pager, .in-list.f-pag');
    if (!pager) return true;
    var lis = allEl(pager, 'li');
    if (lis.length <= 0) return true;
    return lis[lis.length - 1].find('a').length <= 0 ? true : false;
}
//different scock status different colors following HLJ
function getStockColor(stock) {

    if (stock === 'In Stock') return 'background-color: green;';
    else if (stock === 'SALE!') return 'background-color: red;';
    else if (stock === 'Low Stock') return 'background-color: orange;';
    else if (stock === 'Order Stop') return 'background-color: red;';
    else if (stock === 'Product Spotlight') return 'background-color: purple;';
    else if (stock === 'Preorder' || stock.indexOf('Release') >= 0 || stock.indexOf('Restock') >= 0) return 'background-color: blue;';
    return 'background-color: gray;';
}
function allElsAttr(parent, selector, attr) {
    return parent.find(selector).map(function () { return $(this).attr(attr); }).get();
}
function firstEl(parent, selector) {
    var found = parent.find(selector);
    return found.length > 0 ? $(found[0]) : '';
}
function allEl(parent, selector) {
    return Enumerable.from(parent.find(selector)).select(function (x) { return $(x); }).toArray();
}
function getData(url, successFunc) {
    return $.ajax({
        type: 'GET',
        dataType: 'json',
        accept: 'application/json',
        contentType: 'application/json',
        url: url,
        success: successFunc
    });
}
Object.keys = Object.keys || (function () {
    var hasOwnProperty = Object.prototype.hasOwnProperty,
        hasDontEnumBug = !{ toString: null }.propertyIsEnumerable("toString"),
        dontEnums = [
            'toString',
            'toLocaleString',
            'valueOf',
            'hasOwnProperty',
            'isPrototypeOf',
            'propertyIsEnumerable',
            'constructor'
        ];
    return function (o) {
        if (typeof o != "object" && typeof o != "function" || o === null)
            throw new TypeError("Object.keys called on a non-object");
        var result = [];
        for (var name in o) {
            if (hasOwnProperty.call(o, name))
                result.push(name);
        }
        if (hasDontEnumBug) {
            for (var i = 0; i < dontEnums.length; i++) {
                if (hasOwnProperty.call(o, dontEnums[i]))
                    result.push(dontEnums[i]);
            }
        }
        return result;
    };
})();
function pop(obj) {
    for (var key in obj) {
        if (!Object.hasOwnProperty.call(obj, key)) continue;
        var result = obj[key];
        if (!delete obj[key]) { throw new Error(); }
        return result;
    }
    return null;
} 
