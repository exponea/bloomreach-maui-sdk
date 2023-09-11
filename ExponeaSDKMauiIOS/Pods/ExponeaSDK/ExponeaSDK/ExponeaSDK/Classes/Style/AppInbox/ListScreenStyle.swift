//
//  ListScreenStyle.swift
//  ExponeaSDK
//
//  Created by Adam Mihalik on 19/05/2023.
//  Copyright © 2023 Exponea. All rights reserved.
//

import Foundation

public class ListScreenStyle {
    var emptyTitle: TextViewStyle?
    var emptyMessage: TextViewStyle?
    var errorTitle: TextViewStyle?
    var errorMessage: TextViewStyle?
    var progress: ProgressBarStyle?
    var list: AppInboxListViewStyle?

    public init(
        emptyTitle: TextViewStyle? = nil,
        emptyMessage: TextViewStyle? = nil,
        errorTitle: TextViewStyle? = nil,
        errorMessage: TextViewStyle? = nil,
        progress: ProgressBarStyle? = nil,
        list: AppInboxListViewStyle? = nil
    ) {
        self.emptyTitle = emptyTitle
        self.emptyMessage = emptyMessage
        self.errorTitle = errorTitle
        self.errorMessage = errorMessage
        self.progress = progress
        self.list = list
    }

    public func applyTo(_ target: AppInboxListViewController) {
        if let emptyTitleStyle = emptyTitle {
            emptyTitleStyle.applyTo(target.statusEmptyTitle)
        }
        if let emptyMessageStyle = emptyMessage {
            emptyMessageStyle.applyTo(target.statusEmptyMessage)
        }
        if let errorTitleStyle = errorTitle {
            errorTitleStyle.applyTo(target.statusErrorTitle)
        }
        if let errorMessageStyle = errorMessage {
            errorMessageStyle.applyTo(target.statusEmptyMessage)
        }
        if let progressStyle = progress {
            progressStyle.applyTo(target.statusProgress)
        }
        if let listStyle = list {
            listStyle.applyTo(target.tableView)
        }
    }
}
