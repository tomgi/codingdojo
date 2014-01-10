class MQWatchAnalyzer
	def initialize(threshold, result_stream)
		@threshold = threshold
		@result_stream = result_stream
	end

	def analyze(record)
		if @current_date != nil && record.date.hour != @current_date.hour
			@result_stream << "#{@current_date.strftime("%Y.%m.%d %H:00")} 1"
			@current_date = record.date
		end

		if record.count < @threshold
			@current_date = nil 
		elsif record.date.minute == 0
			@current_date = record.date
		end

		if(record.date.minute == 59 && @current_date)
			@result_stream << "#{@current_date.strftime("%Y.%m.%d %H:00")} 1"
		end
	end
end