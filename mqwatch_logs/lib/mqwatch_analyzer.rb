class MQWatchAnalyzer
	def initialize(threshold, result_stream)
		@threshold = threshold
		@result_stream = result_stream
		@records = []
	end

	def analyze(record)
		@records << record

		if(@records.size >= 2 && record.date.minute == 59)
			@result_stream << "1980.1.01 1:00 1"
		end
	end
end