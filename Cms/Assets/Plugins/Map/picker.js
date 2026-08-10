var initLat = "";
var initLng = "";
function onPositionUpdate(position) {
    initLat = position.coords.latitude;
    initLng = position.coords.longitude;
    console.log(initLat + " " + initLng)
}
if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(onPositionUpdate);
}
var elm = $(".map-contaner .loc-init")[0];
initLat = elm.getAttribute("lat");
initLng = elm.getAttribute("lng");
if (initLat == "") {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(onPositionUpdate);
    }
}
if (initLat == "") {
    initLat = "35.699738350000004";
    initLng = "51.33806045";


}
var mapClass = {
    lat: initLat,
    lng: initLng,
    contaner: elm.getAttribute("contaner"),
    latout: elm.getAttribute("latout"),
    lngout: elm.getAttribute("lngout"),
    zoom: elm.getAttribute("zoom"),
    search: elm.getAttribute("search"),
    width: elm.getAttribute("w"),
    height: elm.getAttribute("h")
};
$("[name=" + mapClass.latout + "]").val(initLat);
$("[name=" + mapClass.lngout + "]").val(initLng);
if (mapClass.width != "")
    $(this).css("width", mapClass.width + "px");
if (mapClass.height != "")
    $(this).css("height", mapClass.height + "px");
var map = L.map(mapClass.contaner).setView([mapClass.lat, mapClass.lng], mapClass.zoom);
mapLink =
    '<a href="http://openstreetmap.org">OpenStreetMap</a>';
L.tileLayer(
    'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; ' + mapLink + ' Contributors',
    maxZoom: 18,
}).addTo(map);
if (mapClass.search != "false") {
    map.addControl(new L.Control.Search({
        url: 'https://nominatim.openstreetmap.org/search?format=json&q={s}',
        jsonpParam: 'json_callback',
        propertyName: 'display_name',
        propertyLoc: ['lat', 'lon'],
        autoCollapse: true,
        autoType: false,
        minLength: 2
    }));
}
map.on('moveend', function (e) {
    //مختصات مرکز نقشه 
    var centerLatLng = map.getCenter();
    console.log(centerLatLng.lat + " :" + centerLatLng.lng)
    $("[name=" + mapClass.latout + "]").val(centerLatLng.lat);
    //نوشتن طول جغرافیایی در تکست باکس
    $("[name=" + mapClass.lngout + "]").val(centerLatLng.lng);
});