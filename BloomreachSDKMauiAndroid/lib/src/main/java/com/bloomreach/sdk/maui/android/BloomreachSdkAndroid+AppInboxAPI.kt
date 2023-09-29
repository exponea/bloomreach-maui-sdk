import android.content.Context
import android.widget.Button
import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.exception.BloomreachDataException
import com.bloomreach.sdk.maui.android.util.currentTimeSeconds
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.getNullSafelyMap
import com.bloomreach.sdk.maui.android.util.getRequired
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.models.PurchasedItem

internal fun BloomreachSdkAndroid.getAppInboxButton(context: Context): Button? {
    return Exponea.getAppInboxButton(context);
}
