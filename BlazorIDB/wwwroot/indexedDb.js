function _slicedToArray(t, n) {
    return _arrayWithHoles(t) || _iterableToArrayLimit(t, n) || _unsupportedIterableToArray(t, n) || _nonIterableRest()
}
function _nonIterableRest() {
    throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")
}
function _unsupportedIterableToArray(t, n) {
    if (t) {
        if ("string" == typeof t)
            return _arrayLikeToArray(t, n);
        var r = Object.prototype.toString.call(t).slice(8, -1);
        return "Object" === r && t.constructor && (r = t.constructor.name),
            "Map" === r || "Set" === r ? Array.from(t) : "Arguments" === r || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(r) ? _arrayLikeToArray(t, n) : void 0
    }
}
function _arrayLikeToArray(t, n) {
    (null == n || n > t.length) && (n = t.length);
    for (var r = 0, e = new Array(n); r < n; r++)
        e[r] = t[r];
    return e
}
function _iterableToArrayLimit(t, n) {
    var r = null == t ? null : "undefined" != typeof Symbol && t[Symbol.iterator] || t["@@iterator"];
    if (null != r) {
        var e, o, u = [], i = !0, a = !1;
        try {
            for (r = r.call(t); !(i = (e = r.next()).done) && (u.push(e.value),
                !n || u.length !== n); i = !0)
                ;
        } catch (t) {
            a = !0,
                o = t
        } finally {
            try {
                i || null == r.return || r.return()
            } finally {
                if (a)
                    throw o
            }
        }
        return u
    }
}
function _arrayWithHoles(t) {
    if (Array.isArray(t))
        return t
}
function _typeof(t) {
    return (_typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (t) {
        return typeof t
    }
        : function (t) {
            return t && "function" == typeof Symbol && t.constructor === Symbol && t !== Symbol.prototype ? "symbol" : typeof t
        }
    )(t)
}
!function (t, n) {
    "object" === ("undefined" == typeof exports ? "undefined" : _typeof(exports)) && "undefined" != typeof module ? n(exports) : "function" == typeof define && define.amd ? define(["exports"], n) : n((t = "undefined" != typeof globalThis ? globalThis : t || self).idbKeyVal = {})
}(this, (function (t) {
    "use strict";
    function n(t) {
        return new Promise((function (n, r) {
            t.oncomplete = t.onsuccess = function () {
                return n(t.result)
            }
                ,
                t.onabort = t.onerror = function () {
                    return r(t.error)
                }
        }
        ))
    }
    function r(t, r) {
        var e, o = (!navigator.userAgentData && /Safari\//.test(navigator.userAgent) && !/Chrom(e|ium)\//.test(navigator.userAgent) && indexedDB.databases ? new Promise((function (t) {
            var n = function () {
                return indexedDB.databases().finally(t)
            };
            e = setInterval(n, 100),
                n()
        }
        )).finally((function () {
            return clearInterval(e)
        }
        )) : Promise.resolve()).then((function () {
            var e = indexedDB.open(t);
            return e.onupgradeneeded = function () {
                return e.result.createObjectStore(r)
            }
                ,
                n(e)
        }
        ));
        return function (t, n) {
            return o.then((function (e) {
                return n(e.transaction(r, t).objectStore(r))
            }
            ))
        }
    }
    var e;
    function o() {
        return e || (e = r("keyval-store", "keyval")),
            e
    }
    function u(t, r) {
        return t.openCursor().onsuccess = function () {
            this.result && (r(this.result),
                this.result.continue())
        }
            ,
            n(t.transaction)
    }
    t.clear = function () {
        var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : o();
        return t("readwrite", (function (t) {
            return t.clear(),
                n(t.transaction)
        }
        ))
    }
        ,
        t.createStore = r,
        t.del = function (t) {
            var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : o();
            return r("readwrite", (function (r) {
                return r.delete(t),
                    n(r.transaction)
            }
            ))
        }
        ,
        t.delMany = function (t) {
            var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : o();
            return r("readwrite", (function (r) {
                return t.forEach((function (t) {
                    return r.delete(t)
                }
                )),
                    n(r.transaction)
            }
            ))
        }
        ,
        t.entries = function () {
            var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : o();
            return t("readonly", (function (r) {
                if (r.getAll && r.getAllKeys)
                    return Promise.all([n(r.getAllKeys()), n(r.getAll())]).then((function (t) {
                        var n = _slicedToArray(t, 2)
                            , r = n[0]
                            , e = n[1];
                        return r.map((function (t, n) {
                            return [t, e[n]]
                        }
                        ))
                    }
                    ));
                var e = [];
                return t("readonly", (function (t) {
                    return u(t, (function (t) {
                        return e.push([t.key, t.value])
                    }
                    )).then((function () {
                        return e
                    }
                    ))
                }
                ))
            }
            ))
        }
        ,
        t.get = function (t) {
            var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : o();
            return r("readonly", (function (r) {
                return n(r.get(t))
            }
            ))
        }
        ,
        t.getMany = function (t) {
            var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : o();
            return r("readonly", (function (r) {
                return Promise.all(t.map((function (t) {
                    return n(r.get(t))
                }
                )))
            }
            ))
        }
        ,
        t.keys = function () {
            var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : o();
            return t("readonly", (function (t) {
                if (t.getAllKeys)
                    return n(t.getAllKeys());
                var r = [];
                return u(t, (function (t) {
                    return r.push(t.key)
                }
                )).then((function () {
                    return r
                }
                ))
            }
            ))
        }
        ,
        t.promisifyRequest = n,
        t.set = function (t, r) {
            var e = arguments.length > 2 && void 0 !== arguments[2] ? arguments[2] : o();
            return e("readwrite", (function (e) {
                return e.put(r, t),
                    n(e.transaction)
            }
            ))
        }
        ,
        t.setMany = function (t) {
            var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : o();
            return r("readwrite", (function (r) {
                return t.forEach((function (t) {
                    return r.put(t[1], t[0])
                }
                )),
                    n(r.transaction)
            }
            ))
        }
        ,
        t.update = function (t, r) {
            var e = arguments.length > 2 && void 0 !== arguments[2] ? arguments[2] : o();
            return e("readwrite", (function (e) {
                return new Promise((function (o, u) {
                    e.get(t).onsuccess = function () {
                        try {
                            e.put(r(this.result), t),
                                o(n(e.transaction))
                        } catch (t) {
                            u(t)
                        }
                    }
                }
                ))
            }
            ))
        }
        ,
        t.values = function () {
            var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : o();
            return t("readonly", (function (t) {
                if (t.getAll)
                    return n(t.getAll());
                var r = [];
                return u(t, (function (t) {
                    return r.push(t.value)
                }
                )).then((function () {
                    return r
                }
                ))
            }
            ))
        }
        ,
        Object.defineProperty(t, "__esModule", {
            value: !0
        })
}
));

export function postItem(key, value) {
    idbKeyVal.set(key, value);
}
export function putItem(key, value) {
    idbKeyVal.update(key, val => value);
}
export function removeItem(key) {
    idbKeyVal.del(key);
}
export function getItem(key) {
    return idbKeyVal.get(key);
}
export function clearStore() {
    return idbKeyVal.clear();
}
export function getKeys() {
    return idbKeyVal.keys();
}