/*global $ */

var PENNY = PENNY || {};

PENNY.namespace = function (namespaceString) {
	"use strict";

	var parts = namespaceString.split("."),
		parent = PENNY,
		i;

	if (parts[0] === "PENNY") {
		parts = parts.slice(1);
	}

	for (i = 0; i < parts.length; i += 1) {
		if (parent[parts[i]] === undefined) {
			parent[parts[i]] = {};
		}
		parent = parent[parts[i]];
	}
	return parent;
};