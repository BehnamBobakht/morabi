///*https://github.com/leaflet/Leaflet.markercluster#building-testing-and-linting-scripts/ */

var initLat = "";
var initLng = "";
function onPositionUpdate(position) {
    initLat = position.coords.latitude;
    initLng = position.coords.longitude;
}
function onPositionError() {
    initLat = "35.699738350000004";
    initLng = "51.33806045";
}
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(onPositionUpdate, onPositionError);
    }
}

function showMap() {
    var contaners = $(".map-contaner");
    contaners.each(function (index) {
        var elm = $(this).find(".loc-init");
        var locElms = $(this).children(".loc-item");
        if (elm.attr("lat") !== '' && elm.attr("lng") !== '') {
            initLat = elm.attr("lat");
            initLng = elm.attr("lng");
        }
        else {
            getLocation();
        }
        var mapClass = {
            contaner: elm.attr("contaner"),
            lat: initLat,
            lng: initLng,
            zoom: elm.attr("zoom"),
            search: elm.attr("search"),
            width: elm.attr("w"),
            height: elm.attr("h")
        };
        if (mapClass.width != "")
            $(this).css("width", mapClass.width + "px");
        if (mapClass.height != "")
            $(this).css("height", mapClass.height + "px");
        var locations = [];
        locElms.each(function (index) {

            var loc = [$(this).html(), $(this).attr("lat"), $(this).attr("lng")];
            locations.push(loc);
        });
        var map = L.map(mapClass.contaner).setView([mapClass.lat, mapClass.lng], mapClass.zoom);
        mapLink =
            '<a href="http://openstreetmap.org">OpenStreetMap</a>';
        L.tileLayer(
            'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '',
                maxZoom: 18,
        }).addTo(map);
        var markers = L.markerClusterGroup({ disableClusteringAtZoom: 17 });
        for (var i = 0; i < locations.length; i++) {
 
            var marker = L.marker(L.latLng(locations[i][1], locations[i][2]), { title: locations[i][0] });
            marker.bindPopup(locations[i][0]);
            markers.addLayer(marker);
        }
        map.addLayer(markers);

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
        map.invalidateSize();
        markers.on('click', function (a) {
            var latLngBounds = a.latlng;
            console.log(latLngBounds);
        });
    });

}

$(document).ready(function () {
    showMap();
});
